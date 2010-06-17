using System;

namespace Esath.Pie.Api
{
    public interface IElfEditor
    {
        IElfEditorContext Ctx { get; set; }

        String ElfCode { get; set; }
        event EventHandler ElfCodeChanged;

        void EnterLockedAssignmentMode(String elfCode);
        void LeaveLockedAssignmentMode();
    }
}