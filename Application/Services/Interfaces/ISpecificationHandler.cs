using System.Text;

namespace Application.Services.Interfaces;

public interface ISpecHandler
{
    ISpecHandler SetNext(ISpecHandler next);
    StringBuilder Handle(object spec, StringBuilder sbSoFar);
}