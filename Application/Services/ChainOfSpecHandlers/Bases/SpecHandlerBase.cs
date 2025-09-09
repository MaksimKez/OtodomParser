using System.Text;
using Application.Services.ChainOfSpecHandlers.Interfaces;

namespace Application.Services.ChainOfSpecHandlers.Bases;

public abstract class SpecHandlerBase : ISpecHandler
{
    private ISpecHandler? _next;

    public ISpecHandler SetNext(ISpecHandler next)
    {
        _next = next;
        return next;
    }

    public virtual StringBuilder Handle(object spec, StringBuilder sb)
    {
        sb.Append("/wyniki/");
        return _next != null ? _next.Handle(spec, sb) : sb;
    }
}