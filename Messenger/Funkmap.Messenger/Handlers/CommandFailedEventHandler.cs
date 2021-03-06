﻿using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Logger;
using Funkmap.Messenger.Contracts.Events;

namespace Funkmap.Messenger.Handlers
{
    public class CommandFailedEventHandler : IEventHandler<MessengerCommandFailedEvent>
    {

        private readonly IEventBus _eventBus;
        private readonly IFunkmapLogger<CommandFailedEventHandler> _logger;

        public CommandFailedEventHandler(IEventBus eventBus, IFunkmapLogger<CommandFailedEventHandler> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<MessengerCommandFailedEvent>(Handle);
        }

        public async Task Handle(MessengerCommandFailedEvent @event)
        {
            _logger.Error(@event.Error);
            _logger.Error(@event.ExceptionMessage);
        }
    }
}
