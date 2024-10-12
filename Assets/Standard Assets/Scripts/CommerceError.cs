public class CommerceError
{
	public const int ERROR_BILLING_NOT_SUPPORTED = 100;

	public const int ERROR_INVENTORY_FAILURE_GENERAL = 200;

	public const int ERROR_INVENTORY_FAILURE_NO_ITEMS_RETURNED = 201;

	public const int ERROR_PURCHASE_FAILURE_GENERAL = 300;

	public const int ERROR_PURCHASE_FAILURE_ALREADY_PURCHASED = 301;

	public const int ERROR_PURCHASE_FAILURE_NO_SKU_INFO_ATTEMPT_LOOKUP = 302;

	public const int ERROR_PURCHASE_FAILURE_NO_SKU_INFO_DO_NOT_LOOKUP = 303;

	public const int ERROR_PURCHASE_CANCELLED_BY_USER = 304;

	public const int ERROR_PURCHASE_DEFERRED = 305;

	public const int ERROR_PURCHASE_RESTORE_FAILURE_GENERAL = 400;

	public const int ERROR_PURCHASE_RESTORE_NO_PURCHASES = 401;

	public const int ERROR_PURCHASE_RESTORE_NO_INVENTORY_DETAILS = 402;

	public const int ERROR_BILLING_UNAVAILABLE_UNKNOWN = 500;

	public const int ERROR_BILLING_UNAVAILABLE_NETWORK_CONNECTION = 501;

	public const int ERROR_BILLING_UNAVAILABLE_USER_CANNOT_PURCHASE_OR_API_VERSION_NOT_RECOGNIZED = 502;

	public const int ERROR_BILLING_UNAVAILABLE_DEVELOPER_ERROR = 503;

	public bool _errorFound;

	private int _errorNo;

	private string _errorDesc = string.Empty;

	private string _skuToLookup = string.Empty;

	public CommerceError(int errorNo = 0, string errorDesc = "", string skuToLookup = "")
	{
		if (errorNo != 0)
		{
			SetError(errorNo, errorDesc, skuToLookup);
		}
	}

	public bool HasError()
	{
		return _errorFound;
	}

	public int GetErrorNo()
	{
		return _errorNo;
	}

	public string GetErrorDesc()
	{
		return _errorDesc;
	}

	public string GetSkuToLookup()
	{
		return _skuToLookup;
	}

	public void SetError(int errorNo, string errorDesc = "", string skuToLookup = "")
	{
		_errorFound = true;
		_errorNo = errorNo;
		_errorDesc = errorDesc;
		_skuToLookup = skuToLookup;
	}

	public override string ToString()
	{
		if (_errorFound)
		{
			return $"Error Number: {_errorNo}, Description: {_errorDesc}";
		}
		return "No Error Found";
	}
}
