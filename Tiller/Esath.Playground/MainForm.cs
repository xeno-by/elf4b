using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DataVault.Core.Api;
using Esath.Pie.Contexts;
using Esath.Pie.AstRendering;

namespace Esath.Playground
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
            InitializeComponent();

            _elfEditor.ElfCodeChanged += (o, e) =>
            {
                _elfCode.Text = _elfEditor.ElfCode;
                _publicText.Text = _elfEditor.ElfCode.RenderLightElfAsPublicText(_elfEditor.Ctx);
            };

            using(var vault = VaultApi.OpenZip("scenario.dat"))
            {
                var commonVpath = @"\Scenario\Common";
                var formulaeVpath = @"\Scenario\Particular\cb67b571_0ee8_4087_9b82_743b6d9fc7e9\855f023f_a566_4e85_a24b_c7e59c2e7cff\_sourceValueDeclarations\00d9292a_3e2d_42a8_9182_3091e903ea2d";
                var common = vault.GetBranch(commonVpath).CacheInMemory();
                var formulae = vault.GetBranch(formulaeVpath).CacheInMemory();
                _elfEditor.Ctx = new TillerIntegrationContext(common, formulae, null, null);
//                _elfEditor.Ctx = new TillerIntegrationContext(common);

//                _elfEditor.EnterLockedAssignmentMode(formulae);
//                _elfEditor.LeaveLockedAssignmentMode();
            }

//            _elfEditor.Ctx = new DefaultEditorContext(new []{"var, hallo", "var as well"}
//                .ToDictionary(v => v, v => "*" + v));
//            _elfEditor.ElfCode = "? = ? + 2; a = f(2, ?);";
        }
    }
}
