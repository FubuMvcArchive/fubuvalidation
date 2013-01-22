using FubuMVC.Core.UI.Forms;

namespace FubuMVC.Validation.UI
{
    public interface IRenderingStrategy
    {
        void Modify(FormRequest request);
    }
}