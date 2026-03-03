using CS.DataLibrary.Models.Dictionaries;
using CS.DataLibrary.Repositories;
using CS.LogicLayerLibrary.Interfaces;
using System;
using System.Threading.Tasks;
using CS.MessageHub.Interfaces;
using CS.DataLibrary.Models;

namespace CS.LogicLayerLibrary.Services
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IRepository<CSFont> _csFontRepository;
        private readonly IRepository<CSColor> _csColorRepository;
        private readonly IRepository<CSSubscribeType> _csSubscribeTypeRepository;
        private readonly IMessageService _messageService;

        private const int errorCode = -1;
        private const string AddMessageText = "Запись в таблице была добавлена: ";
        private const string EditMessageText = "Запись в таблице была изменена: ";
        private const string DictionaryNameFontText = "Таблица шрифтов";
        private const string DictionaryNameColorText = "Таблица цветовых палитр";

        public DictionaryService(IRepository<CSFont> csFontRepository, 
            IRepository<CSColor> csColorRepository, 
            IRepository<CSSubscribeType> csSubscribeTypeRepository, 
            IMessageService messageService)
        {
            _csFontRepository = csFontRepository;
            _csColorRepository = csColorRepository;
            _csSubscribeTypeRepository = csSubscribeTypeRepository;
            _messageService = messageService;
        }

        public async Task<int> AddRecordAsync(CSFont model)
        {
            try
            {                                
                var result = await _csFontRepository.AddAsync(model);                
                if (result > 0)
                {
                    var messageText = string.Concat(AddMessageText, DictionaryNameFontText, " - ", model.Name);
                    await SendMessage(messageText, "Font");
                    return result;
                }
            }
            catch
            {                
            }
            return errorCode;
        }

        public async Task<int> AddRecordAsync(CSColor model)
        {
            try
            {
                var result = await _csColorRepository.AddAsync(model);
                if (result > 0)
                {
                    var messageText = string.Concat(AddMessageText, DictionaryNameColorText, " - ", model.PrimaryColor, "-", model.SecondaryColor);
                    await SendMessage(messageText, "Color");
                    return result;
                }
            }
            catch
            {
            }
            return errorCode;
        }

        public async Task<int> EditRecordAsync(CSFont model)
        {
            try
            {                
                var result = await _csFontRepository.UpdateAsync(model);
                if (result > 0)
                {
                    var messageText = string.Concat(EditMessageText, DictionaryNameFontText, " - ", model.Name);
                    await SendMessage(messageText, "Font");
                    return result;
                }
            }
            catch
            {
            }
            return errorCode;
        }

        public async Task<int> EditRecordAsync(CSColor model)
        {
            try
            {
                var result = await _csColorRepository.UpdateAsync(model);
                if (result > 0)
                {
                    var messageText = string.Concat(EditMessageText, DictionaryNameColorText, " - ", model.PrimaryColor, "-", model.SecondaryColor);
                    await SendMessage(messageText, "Color");
                    return result;
                }
            }
            catch
            {
            }
            return errorCode;
        }

        private async Task SendMessage(string messageText, string subscribeType)
        {
            var subscribe = await _csSubscribeTypeRepository.GetFirstOrDefaultAsync(x => x.Type == subscribeType);
            if (subscribe is not null)
            {
                var message = new CSMessage()
                {
                    Content = messageText,
                    SubscribeId = subscribe.Id
                };

                await _messageService.SendMessageAsync(message);
            }
        }
    }
}

