using Application.Services.ChainOfSpecHandlers;

namespace Application.Services.Interfaces;

public interface ISpecHandlerChainFactory
{
    BaseSpecificationsHandler Create();
}
