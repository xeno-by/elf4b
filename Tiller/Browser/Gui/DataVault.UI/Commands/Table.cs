using System;
using Browser.Properties;
using DataVault.Core.Helpers;
using System.Linq;
using DataVault.UI.Api.Exceptions;
using DataVault.UI.Api.ContentTypez;

namespace Browser.Gui.DataVault.UI.Commands
{
	using global::DataVault.Core.Helpers.Assertions;

	public class Table
    {
        private String[,] _content;
        public String[] Columns { get; private set; }
        public IContentType[] ColumnTypes { get; private set; }
        public String[] Rows { get; private set; }
        public String[][] Data { get; private set; }

        public Table(String[,] content)
        {
            _content = content;
            var jagged = content.ToJagged();
            (jagged.Length > 1).AssertTrue();
            (jagged[0].Length > 1).AssertTrue();
            (jagged.Min(dim => dim.Length) == jagged.Max(dim => dim.Length)).AssertTrue();

            Columns = jagged[0].Skip(1).Select(s => s.Substring(0, s.IndexOf(':'))).ToArray();
            Rows = jagged.Skip(1).Select(dim => dim[0]).ToArray();
            Data = jagged.Skip(1).Select(dim => dim.Skip(1).ToArray()).ToArray();

            var ctypes = jagged[0].Skip(1).Select(s => s.Substring(s.IndexOf(':') + 1)).ToArray();
            ColumnTypes = ctypes.Select((ctype, i) => {
                var t = ContentTypes.All.SingleOrDefault(w => String.Compare(
                    w.LocTypeName, ctype.ToLower(), true) == 0);
                if (t == null)
                {
                    var s_wrappers = ContentTypes.All.Select(w => w.LocTypeName);
                    throw new ValidationException(Resources.TableImport_BadFormatOfColumnHeader, 
                        ctype, s_wrappers.StringJoin());
                }

                return t;
            }).ToArray();
        }

        public override String ToString()
        {
            return _content.ToJagged().Select(dim => dim.StringJoin("\t")).StringJoin(Environment.NewLine);
        }
    }
}