using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;

namespace CitiesTelegramBot
{
    class Program
    {
        static ITelegramBotClient botClient;
        static List<Game> GamePool;
        static readonly string tokenPath = @"C:\Just_Projects\C#\TelegramBots\token.txt";

        static void Main()
        {
            GamePool = new List<Game>();

            DBLite.Initialization();
            DBLite.CreateAndConnectDB();
            //DBLite.ReadAll();
            //DBLite.Add();
            //DBLite.ReadAll();


            botClient = new TelegramBotClient(System.IO.File.ReadAllText(tokenPath));
            var me = botClient.GetMeAsync().Result;

            Console.WriteLine($"Hello, World! I am bot - {me.Id} and my name is {me.FirstName}.");

            botClient.OnMessage += Bot_OnMessage;

            botClient.StartReceiving();

            //CheckConnection(client);

            Console.WriteLine($"Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Text != null)
            {
                Game res = null;
                if (GamePool.Count != 0)
                    res = GamePool.Find(x => x.Player.Id == message.Chat.Id);

                if (res != null)
                {
                    switch (message.Text)
                    {
                        case "Подсказка":
                            await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "Подумай пока сам)"
                            );
                            break;
                        case "Закончить игру":
                            await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "В данный момент игра вечна (МУХАХА)"
                            );
                            break;
                        default:
                            Console.WriteLine($"Received a text message ({message.Text}) in chat {message.Chat.Id} ({message.Chat.FirstName} {message.Chat.LastName})");

                            string cityname = message.Text.Trim().First().ToString().ToUpper() + message.Text.Trim().Substring(1).ToLower();

                            var cityobj = DBLite.FindCity(cityname);

                            if (cityobj != null)
                            {
                                uint id = Convert.ToUInt32(cityobj[0]);
                                string name = cityobj[3].ToString();

                                if (res.AddCity(id))
                                {
                                    res.LastCity = name;

                                    if (DBLite.FindCityByLetter(res))
                                    {
                                        await botClient.SendTextMessageAsync(
                                        chatId: message.Chat,
                                        text: "Хм.. " + res.LastCity + ". Вам на букву " + res.LastLetter.ToString().ToUpper(),
                                        replyMarkup: KeyBoards.gamemenu
                                        );
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(
                                        chatId: message.Chat,
                                        text: "Произошла какая-то ошибка (Возможно города на эту букву закончились)"
                                        );
                                    }
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                                                chatId: message.Chat,
                                                                text: "Мы уже называли такой город. Попробуйте ещё"
                                                                );
                                }
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(
                                                            chatId: message.Chat,
                                                            text: "Кажется такого города не существует"
                                                            );
                            }

                            //Console.WriteLine($"{msg.From.FirstName} sent message {msg.MessageId} " +
                            //    $"to chat {msg.Chat.Id} at {msg.Date}.");
                            break;

                    }


                }
                else
                {
                    switch (message.Text)
                    {
                        case "/start":
                            await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "Привет, с этим ботом Вы можете поиграть в \"Города\"",
                            replyMarkup: KeyBoards.mainmenu);
                            break;

                        case "Начать игру":
                            var temp = new Game(message.Chat.FirstName, message.Chat.Id);

                            object[] city = DBLite.RandomCity();
                            uint id = Convert.ToUInt32(city[0]);
                            string name = city[3].ToString();

                            temp.AddCity(id);
                            temp.LastCity = name;

                            GamePool.Add(temp);


                            await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "Игра начинается!\nПервый город:" + name + ".\nНазовите город на букву " + temp.LastLetter.ToString().ToUpper(),
                            replyMarkup: KeyBoards.gamemenu
                            );
                            break;

                        case "О боте":
                            await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "Данный бот обучен игре в \"Города\""
                            );
                            break;
                        case "Помощь":
                            await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "При возникновении ошибок напишите разработчику (@aolta)"
                            );
                            break;
                        default:
                            await botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "Не совсем понятно, что Вы хотели сказать этим"
                            );
                            break;
                    }
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text: "Может вы лучше напишите мне что-то?"
                );
            }
        }
    }
}
