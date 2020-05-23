namespace System
{
    internal static class ObjectExtensions
    {
		internal static object GetPropertyValue(this object o, string propertyName)
		{
			object objValue = string.Empty;

			var propertyInfo = o.GetType().GetProperty(propertyName);

			if (propertyInfo != null)
            {
				objValue = propertyInfo.GetValue(o, null);
			}

			return objValue;
		}
	}
}
