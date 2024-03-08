using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("6549517652:AAHIeBdM3KZxiYs0YjHfSxICOu2FtEqNFZU");

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandlerUpdateAsync,
    pollingErrorHandler: HandPollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
    );

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message) return;
    if (message.Text is not { } messageText) return;

    var chatId = message.Chat.Id;

    chatId = update.Message.Chat.Id;
    messageText = update.Message.Text;
    var messageId = update.Message.MessageId;
    var firstName = update.Message.From.FirstName;
    var lastName = update.Message.From.LastName;
    var id = update.Message.From.Id;
    var username = update.Message.From.Username;
    var year = update.Message.Date.Year;
    var month = update.Message.Date.Month;
    var day = update.Message.Date.Day;
    var hour = update.Message.Date.Hour;
    var minute = update.Message.Date.Minute;
    var second = update.Message.Date.Second;

    Console.WriteLine("\nData message --> " + year + "/" + month + "/" + day + " - " + hour + ":" + minute + ":" + second);
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} from user:\n" + firstName + " - " + lastName + " - " + "5873853");

    messageText = messageText.ToLower();

    if (messageText != null && int.Parse(day.ToString()) >= day && int.Parse(hour.ToString()) >= hour && int.Parse(minute.ToString()) >= minute &&
        int.Parse(second.ToString()) >= second - 10)
    {
        var getChatMember = await botClient.GetChatMemberAsync("@LINQMethods", id);

        if (messageText == "/start")
        {
            if (getChatMember.Status.ToString() != "Member")
            {
                await UserIsSubscriber(botClient, id, cancellationToken);
            }
        }
        else if (messageText == "/home")
        {
            ReplyKeyboardMarkup r = new(new[]
{
    KeyboardButton.WithRequestLocation("Share Location"),
    KeyboardButton.WithRequestContact("Share Contact"),
});
            Message se= await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Who or Where are you?",
    replyMarkup: r,
    cancellationToken: cancellationToken);
        }
    }
}

Task HandPollingErrorAsync(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
        => $"Telegram api error:\n [{apiRequestException.ErrorCode}]\n {apiRequestException.Message}",
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

async Task UserIsSubscriber (ITelegramBotClient telegramBotClient, long id, CancellationToken cancellationToken)
{
    InlineKeyboardMarkup inlineKeyboard = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithUrl(text: "Canal: ", url: "https://t.me/https://t.me/LINQMethods2")
        }
    });

    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: id,
        text: "Before use the bot you must follow this channels.",
        replyMarkup: inlineKeyboard,
        cancellationToken: cancellationToken);
}