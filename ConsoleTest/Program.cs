using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1_playingcardLibrary;

namespace ConsoleTest
{
    internal class Program
    {
        public void Main(string[] args)
        {
            Deck deck = new Deck();
            deck.Shuffle();
            PrintDeck(deck);
            deck.Shuffle();
            
            
            Console.ReadLine();
        }

        public void PrintDeck(Deck deck)
        {
            int count = 0;
            while (!deck.IsEmpty)
            {
                count++;
                Console.WriteLine(deck.DealTopCard());
            }
            Console.WriteLine(count);
        }

    }
}
