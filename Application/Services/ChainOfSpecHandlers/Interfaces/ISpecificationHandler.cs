using System.Text;

namespace Application.Services.ChainOfSpecHandlers.Interfaces;

public interface ISpecHandler
{
    ISpecHandler SetNext(ISpecHandler next);
    StringBuilder Handle(object spec, StringBuilder sbSoFar);
}