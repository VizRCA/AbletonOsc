using System;
using System.ComponentModel;
using System.Reflection;

namespace AbletonOsc
{
	public class EnumUtils<T>
	{
		public static string GetDescription (T enumValue, string defDesc)
		{
		
			FieldInfo fi = enumValue.GetType ().GetField (enumValue.ToString ());
		
			if (null != fi) {
				object[] attrs = fi.GetCustomAttributes
				(typeof(DescriptionAttribute), true);
				if (attrs != null && attrs.Length > 0)
					return ((DescriptionAttribute)attrs [0]).Description;
			}
		
			return defDesc;
		}
	
		public static string GetDescription (T enumValue)
		{
			return GetDescription (enumValue, string.Empty);
		}
	
		public static T FromDescription (string description)
		{
			Type t = typeof(T);
			foreach (FieldInfo fi in t.GetFields()) {
				object[] attrs = fi.GetCustomAttributes
				(typeof(DescriptionAttribute), true);
				if (attrs != null && attrs.Length > 0) {
					foreach (DescriptionAttribute attr in attrs) {
						if (attr.Description.Equals (description))
							return (T)fi.GetValue (null);
					}
				}
			}
			return default(T);
		}

        public static string GetCommand(T enumValue, string defDesc)
        {

            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes
                                   (typeof(OscCommandAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((OscCommandAttribute)attrs[0]).Command;
            }

            return defDesc;
        }

        public static string GetCommand(T enumValue)
        {
            return GetCommand(enumValue, string.Empty);
        }

        public static T FromCommand(string command)
        {
            Type t = typeof(T);
            foreach (FieldInfo fi in t.GetFields())
            {
                object[] attrs = fi.GetCustomAttributes
                                   (typeof(OscCommandAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    foreach (OscCommandAttribute attr in attrs)
                    {
                        if (attr.Command.Equals(command))
                            return (T)fi.GetValue(null);
                    }
                }
            }
            return default(T);
        }
	}

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class OscCommandAttribute : Attribute
    {
        public string Command;
        public OscCommandAttribute(string cmd){
            this.Command = cmd;
        }
    }
}