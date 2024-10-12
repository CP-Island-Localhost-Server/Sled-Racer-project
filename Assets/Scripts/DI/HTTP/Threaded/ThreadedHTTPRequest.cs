using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DI.HTTP.Security;
using DI.HTTP.Security.Pinning;
using DI.Threading;
using UnityEngine; // Using UnityEngine for Unity's Debug class

namespace DI.HTTP.Threaded
{
    public class ThreadedHTTPRequest : HTTPBaseRequestImpl, IHTTPRequest
    {
        public static int DOWNLOAD_BUFFER_SIZE = 1024;

        private static string[] sizes = new string[4] { "B", "KB", "MB", "GB" };

        public ThreadedHTTPRequest(ThreadedHTTPClient client)
            : base(client)
        {
        }

        public override IHTTPResponse performSync()
        {
            return send();
        }

        public override void performAsync()
        {
            UnityThreadHelper.CreateThread((Action)delegate
            {
                send();
            });
        }

        public override string getUrl()
        {
            lock (this)
            {
                return base.getUrl();
            }
        }

        public override void setUrl(string url)
        {
            lock (this)
            {
                base.setUrl(url);
            }
        }

        public static bool ThreadSafeBaseValidateCertificate(RequestDigestSet requestDigestSet, X509Certificate certificate, SslPolicyErrors sslPolicyError)
        {
            if (certificate != null)
            {
                DigestSet digestSet = new DigestSet();
                byte[] rawCertData = certificate.GetRawCertData();
                digestSet.setSha1(DigestHelper.sha1(rawCertData));
                digestSet.setSha256(DigestHelper.sha256(rawCertData));
                requestDigestSet.CertificateDigest = digestSet;
                digestSet = new DigestSet();
                rawCertData = certificate.GetPublicKey();
                digestSet.setSha1(DigestHelper.sha1(rawCertData));
                digestSet.setSha256(DigestHelper.sha256(rawCertData));
                requestDigestSet.SubjectDigest = digestSet;
                return true;
            }
            return false;
        }

        public static bool ThreadSafeValidateCertificate(IPinset pinset, HttpWebRequest httpWebRequest, X509Certificate certificate, SslPolicyErrors sslPolicyErrors)
        {
            // Bypass certificate validation and always return true
            return true;
        }

        public bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Override to always return true, bypassing SSL/TLS validation errors
            return true;
        }

        private IHTTPResponse send()
        {
            UnityEngine.Debug.Log("Bypassing server request to prevent TLS errors."); // Specify UnityEngine.Debug

            // Create a fake HTTP response indicating success to prevent further network attempts
            HTTPBaseResponseImpl fakeResponse = new HTTPBaseResponseImpl(this);
            fakeResponse.setStatusCode(200); // HTTP 200 OK
            fakeResponse.setReasonPhrase("Success");

            OnSuccess(fakeResponse); // Simulate a successful response
            OnComplete();
            return fakeResponse;
        }

        public override void OnStart()
        {
            IHTTPListener listener = getEffectiveListener();
            if (listener != null)
            {
                UnityThreadHelper.Dispatcher.Dispatch(delegate
                {
                    listener.OnStart(this);
                });
            }
        }

        public override void OnProgress(byte[] data, int bytesRead, int bytesReceived, int bytesExpected)
        {
            IHTTPListener listener = getEffectiveListener();
            if (listener != null)
            {
                UnityThreadHelper.Dispatcher.Dispatch(delegate
                {
                    listener.OnProgress(this, data, bytesRead, bytesReceived, bytesExpected);
                });
            }
        }

        public override void OnSuccess(IHTTPResponse response)
        {
            IHTTPListener listener = getEffectiveListener();
            if (listener != null)
            {
                UnityThreadHelper.Dispatcher.Dispatch(delegate
                {
                    listener.OnSuccess(this, response);
                });
            }
        }

        public override void OnError(IHTTPResponse response, Exception exception)
        {
            IHTTPListener listener = getEffectiveListener();
            if (listener != null)
            {
                UnityThreadHelper.Dispatcher.Dispatch(delegate
                {
                    listener.OnError(this, response, exception);
                });
            }
        }

        public override void OnComplete()
        {
            IHTTPListener listener = getEffectiveListener();
            if (listener != null)
            {
                UnityThreadHelper.Dispatcher.Dispatch(delegate
                {
                    listener.OnComplete(this);
                });
            }
        }

        protected static void logFailure(PinningMode mode, PinningTarget target)
        {
            UnityEngine.Debug.LogWarning(string.Concat("Failed to validate certificate against pinset. mode=", mode, ", target=", target.ToString())); // Specify UnityEngine.Debug
        }

        public string InfoString(HttpWebRequest httpWebRequest, byte[] requestBytes, HttpWebResponse httpWebResponse, byte[] responseBytes, long responseTime, bool verbose)
        {
            Uri requestUri = httpWebRequest.RequestUri;
            string text = httpWebRequest.Method.ToString();
            string text2 = ((httpWebResponse != null) ? Convert.ToString((int)httpWebResponse.StatusCode) : "---");
            string text3 = ((httpWebResponse != null) ? httpWebResponse.StatusDescription : "Unknown");
            double num = ((responseBytes != null) ? ((float)responseBytes.Length) : 0f);
            int num2 = 0;
            while (num >= 1024.0 && num2 + 1 < sizes.Length)
            {
                num2++;
                num /= 1024.0;
            }
            string text4 = string.Format("{0:0.##}{1}", num, sizes[num2]);
            string text5 = requestUri.ToString() + " [ " + text.ToUpper() + " ] [ " + text2 + " " + text3 + " ] [ " + text4 + " ] [ " + responseTime + "ms ]\n";
            if (verbose)
            {
                text5 = text5 + "\nRequest Headers:\n\n" + httpWebRequest.Headers.ToString();
                if (requestBytes != null)
                {
                    text5 = text5 + "\nRequest Body:\n\n" + Encoding.UTF8.GetString(requestBytes);
                }
                text5 = ((httpWebResponse == null || httpWebResponse.Headers == null) ? (text5 + "n/a") : (text5 + "\n\nResponse Headers:\n\n" + httpWebResponse.Headers.ToString()));
                if (responseBytes != null)
                {
                    text5 = text5 + "\nResponse Body:\n\n" + Encoding.UTF8.GetString(responseBytes);
                }
                text5 += "\n";
            }
            return text5;
        }
    }
}
