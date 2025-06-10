// See https://aka.ms/new-console-template for more information
using arrayutils;
using ConsoleUtils;
using tictactoe;
using tictactoe.Game;
using tictactoe.Game.GameObjects;

Game game = new Game(new string[] { "x", "o" }, 3, 3, 3, 3);
game.Play();