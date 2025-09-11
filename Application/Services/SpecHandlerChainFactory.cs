using Application.Services.ChainOfSpecHandlers;
using Application.Services.Interfaces;

namespace Application.Services;

public class SpecHandlerChainFactory(
    BaseSpecificationsHandler baseHandler,
    DefaultSpecificationsHandler defaultHandler,
    ExactSpecificationsHandler exactHandler)
    : ISpecHandlerChainFactory
{
    public BaseSpecificationsHandler Create()
    {
        // base -> default -> exact
        baseHandler
            .SetNext(defaultHandler)
            .SetNext(exactHandler);

        return baseHandler;
    }
}
