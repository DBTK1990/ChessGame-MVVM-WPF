﻿using Chess0.Model;
using System.Windows.Input;
using Chess0.Helper;
using Chess0.Model.Peices;
using System.Collections.Generic;
using System;
using Chess0.ViewModel.Chess;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace Chess0.ViewModel
{
    public enum CompassE { North, North_East, East, South_East, South, South_Wast, Wast, North_Wast };

    class BoardViewModel
    {
        private ObservableBoardCollection<TileModel> tiles;
        public ObservableBoardCollection<TileModel> Tiles
        {
            get
            {
                return tiles;
            }
            set
            {
                tiles = value;
            }
        }
        private ObservableCollection<IPieceModel> dead_white;
        public ObservableCollection<IPieceModel> DeadWhite
        {
            get
            {
                return dead_white;
            }
            set
            {
                dead_white = value;
            }
        }


        private ObservableCollection<IPieceModel> dead_black;
        public ObservableCollection<IPieceModel> DeadBlack
        {
            get
            {
                return dead_black;
            }
            set
            {
                dead_black = value;
            }
        }

        private TileModel focus;
        public TileModel Focus {
            get
            {
                return focus;
            }
            set
            {
                focus = value;
            }
        }

        private State playerturn;
        public State PlayerTurn
        {
            get
            {
                return playerturn;
            }
            set
            {
                playerturn = value;
            }
        }
        public string PlayerTurnS
        {
            get
            {
                return playerturn.ToString();
            }
        }

        private ICommand buttoncommand;
        public ICommand ButtonCommand
        {
            get
            {
                return buttoncommand;

            }


            private set
            {
                buttoncommand = value;
            }
        }

        private ICommand restartcommand;
        public ICommand RestartCommand
        {
            get
            {
                return restartcommand;

            }


            private set
            {
                restartcommand = value;
            }
        }


        public BoardViewModel() 
        {
            Tiles = new ObservableBoardCollection<TileModel>(8);
          
            Focus = null;

            InitBoard();
            InitPieces();
            DeadBlack = new ObservableCollection<IPieceModel>();
            dead_white = new ObservableCollection<IPieceModel>();

            PlayerTurn = State.Black;

           // testdeadPieces();
           
            ButtonCommand = new RelayCommand(MyOnClick);
            RestartCommand = new RelayCommand(RestartGame);
            //setgamerules();



        }

        private void InitBoard()
        {
            for (var LoopIndex = 0; LoopIndex < 64; LoopIndex++)
            {
                TileModel tile;

                int RowIndex = (int)(LoopIndex / 8);
                int ColIndex = LoopIndex % 8;

                if (RowIndex % 2 == 0)
                    tile = (ColIndex % 2 == 0) ? new TileModel("BurlyWood", new MyPoint(RowIndex, ColIndex)) : new TileModel("#FF876539", new MyPoint(RowIndex, ColIndex));
                else
                    tile = (ColIndex % 2 == 0) ? new TileModel("#FF876539", new MyPoint(RowIndex, ColIndex)) : new TileModel("BurlyWood", new MyPoint(RowIndex, ColIndex));

              
                Tiles[RowIndex, ColIndex] = tile;
            }

        }

        private void RestartGame(object ob)
        {

            DeadWhite.Clear();
            DeadBlack.Clear();

            for (var LoopIndex = 0; LoopIndex < 64; LoopIndex++)
            {
                Tiles[LoopIndex].Piece = null;
            }

            InitPieces();

            DialogHost.CloseDialogCommand.Execute(null, null);

        }

        private void testdeadPieces()
        {
            ChessPlayer white = new ChessPlayer(State.White);
            ChessPlayer black = new ChessPlayer(State.Black);

            for (var i = 0; i < white.Pieces.Count; i++)
            {
                DeadWhite.Add( white.Pieces[i]);
                DeadBlack.Add( black.Pieces[i]);
            }

        }

        private void InitPieces()
        {
            ChessPlayer white = new ChessPlayer(State.White);
            ChessPlayer black = new ChessPlayer(State.Black);

            for (var i = 0; i < white.Pieces.Count; i++)
            {
                Tiles[white.Pieces[i].Pos].Piece = white.Pieces[i];
                Tiles[black.Pieces[i].Pos].Piece = black.Pieces[i];

            }
        }

        private void MyOnClick(object o)
        {
            MyPoint tileIndex= (MyPoint)o;


            if (Focus != null && Tiles[tileIndex].MarkVisibility == "Visiable")
            {
                if (Tiles[tileIndex].MarkColor == "Green")
                {
                    MovePiece(Focus.Pos, tileIndex);
                }
                else if (Tiles[tileIndex].MarkColor == "Red")
                {
                    EatPiece(Focus.Pos, tileIndex);
                }

                PlayerTurn=Rules_Chess.PlayerTurnSwitch(ref focus, ref tiles);
            }
            else if (Tiles[tileIndex].Piece!=null)
                {
                if (Focus == null && Tiles[tileIndex].Piece.Player == PlayerTurn || (Focus != null && Tiles[tileIndex].Piece.Player == PlayerTurn))
                {
                    Focus = Tiles[tileIndex];
                    Rules_Chess.SimulatePath(Focus,ref tiles);
                }
                }

        }


        //rules function

       
        /// <summary>
        ///queen is therthend????
        /// </summary>
        




        public void MovePiece(MyPoint point ,MyPoint moveTo)
        {

            Tiles[moveTo].Piece= Tiles[point].Piece;
            Tiles[point].Piece.Pos = moveTo;
            
            Tiles[point].Piece = null;

            Tiles[moveTo].Piece.MovesMade++;

            Focus = Tiles[moveTo];
            
        
        }

       
        public  void EatPiece(MyPoint point, MyPoint moveTo)
        {
            if (Tiles[moveTo].Piece is Piece_Queen_M)
            {
                GameOverModel gameOver = new GameOverModel(this.PlayerTurnS);
                DialogHost.OpenDialogCommand.Execute(gameOver, null);
               // DialogHost.Show(gameOver, "GameOver");
                
            }


            switch (Tiles[moveTo].Piece.Player)
            {
                case State.Black:
                    DeadBlack.Add(Tiles[moveTo].Piece);
                    break;
                case State.White:
                    DeadWhite.Add(Tiles[moveTo].Piece);
                    break;

            }

            
                Tiles[moveTo].Piece = Tiles[point].Piece;
                Tiles[point].Piece.Pos = moveTo;

                Tiles[point].Piece = null;

                Tiles[moveTo].Piece.MovesMade++;

                Focus = Tiles[moveTo];
            
           

        }

        
    }
}
