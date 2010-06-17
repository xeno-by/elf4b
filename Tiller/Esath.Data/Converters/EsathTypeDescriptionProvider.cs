using System;
using System.ComponentModel;
using System.Linq;

namespace Esath.Data.Converters
{
    public class EsathTypeDescriptionProvider : TypeDescriptionProvider
    {
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new EsathTypeDescriptor(objectType);
        }

        private class EsathTypeDescriptor : CustomTypeDescriptor
        {
            public Type ObjectType { get; private set; }

            public EsathTypeDescriptor(Type objectType)
            {
                ObjectType = objectType;
            }

            public override TypeConverter GetConverter()
            {
                if (ObjectType != null && ObjectType.IsDefined(typeof(TypeConverterAttribute), true))
                {
                    var cnvtn = ((TypeConverterAttribute)ObjectType.GetCustomAttributes(
                        typeof(TypeConverterAttribute), true).Single()).ConverterTypeName;
                    var cnvt = Type.GetType(cnvtn);

                    var cnv = (TypeConverter)Activator.CreateInstance(cnvt);
                    if (cnv is EsathConverter)
                    {
                        ((EsathConverter)cnv).ObjectType = ObjectType;
                    }

                    return cnv;
                }
                else
                {
                    return base.GetConverter();
                }
            }
        }
    }
}