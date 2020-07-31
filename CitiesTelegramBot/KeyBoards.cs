using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CitiesTelegramBot
{
    static class KeyBoards
    {
        public static ReplyKeyboardMarkup mainmenu = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
                                {
                                new[]
                                {
                                        new KeyboardButton("Начать игру")
                                },
                                new[]
                                {
                                        new KeyboardButton("О боте"),

                                        new KeyboardButton("Помощь")
                                },
                            },
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup gamemenu = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
                                {
                                new[]
                                {
                                        new KeyboardButton("Подсказка"),

                                        new KeyboardButton("Закончить игру")
                                },
                            },
            ResizeKeyboard = true
        };
    }
}
