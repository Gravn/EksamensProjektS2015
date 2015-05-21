﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Input
{

    public enum Modifiers
        {
            Control = 1,
            Shift = 2,
            Alt = 4,
            None = 0
        };

    public static class TypingKeyboard
    {
        public static char? ToChar(Keys key, Modifiers modifiers = Modifiers.None)
        {
            if (key == Keys.A) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'A' : 'a'; }
            if (key == Keys.B) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'B' : 'b'; }
            if (key == Keys.C) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'C' : 'c'; }
            if (key == Keys.D) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'D' : 'd'; }
            if (key == Keys.E) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'E' : 'e'; }
            if (key == Keys.F) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'F' : 'f'; }
            if (key == Keys.G) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'G' : 'g'; }
            if (key == Keys.H) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'H' : 'h'; }
            if (key == Keys.I) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'I' : 'i'; }
            if (key == Keys.J) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'J' : 'j'; }
            if (key == Keys.K) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'K' : 'k'; }
            if (key == Keys.L) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'L' : 'l'; }
            if (key == Keys.M) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'M' : 'm'; }
            if (key == Keys.N) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'N' : 'n'; }
            if (key == Keys.O) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'O' : 'o'; }
            if (key == Keys.P) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'P' : 'p'; }
            if (key == Keys.Q) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'Q' : 'q'; }
            if (key == Keys.R) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'R' : 'r'; }
            if (key == Keys.S) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'S' : 's'; }
            if (key == Keys.T) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'T' : 't'; }
            if (key == Keys.U) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'U' : 'u'; }
            if (key == Keys.V) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'V' : 'v'; }
            if (key == Keys.W) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'W' : 'w'; }
            if (key == Keys.X) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'X' : 'x'; }
            if (key == Keys.Y) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'Y' : 'y'; }
            if (key == Keys.Z) { return ((modifiers & Modifiers.Shift) == Modifiers.Shift) ? 'Z' : 'z'; }

            if ((key == Keys.D0 && !ShiftDown(modifiers)) || key == Keys.NumPad0) { return '0'; }
            if ((key == Keys.D1 && !ShiftDown(modifiers)) || key == Keys.NumPad1) { return '1'; }
            if ((key == Keys.D2 && !ShiftDown(modifiers)) || key == Keys.NumPad2) { return '2'; }
            if ((key == Keys.D3 && !ShiftDown(modifiers)) || key == Keys.NumPad3) { return '3'; }
            if ((key == Keys.D4 && !ShiftDown(modifiers)) || key == Keys.NumPad4) { return '4'; }
            if ((key == Keys.D5 && !ShiftDown(modifiers)) || key == Keys.NumPad5) { return '5'; }
            if ((key == Keys.D6 && !ShiftDown(modifiers)) || key == Keys.NumPad6) { return '6'; }
            if ((key == Keys.D7 && !ShiftDown(modifiers)) || key == Keys.NumPad7) { return '7'; }
            if ((key == Keys.D8 && !ShiftDown(modifiers)) || key == Keys.NumPad8) { return '8'; }
            if ((key == Keys.D9 && !ShiftDown(modifiers)) || key == Keys.NumPad9) { return '9'; }

            if (key == Keys.D0 && ShiftDown(modifiers)) { return ')'; }
            if (key == Keys.D1 && ShiftDown(modifiers)) { return '!'; }
            if (key == Keys.D2 && ShiftDown(modifiers)) { return '@'; }
            if (key == Keys.D3 && ShiftDown(modifiers)) { return '#'; }
            if (key == Keys.D4 && ShiftDown(modifiers)) { return '$'; }
            if (key == Keys.D5 && ShiftDown(modifiers)) { return '%'; }
            if (key == Keys.D6 && ShiftDown(modifiers)) { return '^'; }
            if (key == Keys.D7 && ShiftDown(modifiers)) { return '&'; }
            if (key == Keys.D8 && ShiftDown(modifiers)) { return '*'; }
            if (key == Keys.D9 && ShiftDown(modifiers)) { return '('; }

            if (key == Keys.Space) { return ' '; }
            if (key == Keys.Tab) { return '\t'; }
            if (key == Keys.Enter) { return '\n'; }

            if (key == Keys.Add) { return '+'; }
            if (key == Keys.Decimal) { return '.'; }
            if (key == Keys.Divide) { return '/'; }
            if (key == Keys.Multiply) { return '*'; }
            if (key == Keys.OemBackslash) { return '\\'; }
            if (key == Keys.OemComma && !ShiftDown(modifiers)) { return ','; }
            if (key == Keys.OemComma && ShiftDown(modifiers)) { return '<'; }
            if (key == Keys.OemOpenBrackets && !ShiftDown(modifiers)) { return '['; }
            if (key == Keys.OemOpenBrackets && ShiftDown(modifiers)) { return '{'; }
            if (key == Keys.OemCloseBrackets && !ShiftDown(modifiers)) { return ']'; }
            if (key == Keys.OemCloseBrackets && ShiftDown(modifiers)) { return '}'; }
            if (key == Keys.OemPeriod && !ShiftDown(modifiers)) { return '.'; }
            if (key == Keys.OemPeriod && ShiftDown(modifiers)) { return '>'; }
            if (key == Keys.OemPipe && !ShiftDown(modifiers)) { return '\\'; }
            if (key == Keys.OemPipe && ShiftDown(modifiers)) { return '|'; }
            if (key == Keys.OemPlus && !ShiftDown(modifiers)) { return '='; }
            if (key == Keys.OemPlus && ShiftDown(modifiers)) { return '+'; }
            if (key == Keys.OemMinus && !ShiftDown(modifiers)) { return '-'; }
            if (key == Keys.OemMinus && ShiftDown(modifiers)) { return '_'; }
            if (key == Keys.OemQuestion && !ShiftDown(modifiers)) { return '/'; }
            if (key == Keys.OemQuestion && ShiftDown(modifiers)) { return '?'; }
            if (key == Keys.OemQuotes && !ShiftDown(modifiers)) { return '\''; }
            if (key == Keys.OemQuotes && ShiftDown(modifiers)) { return '"'; }
            if (key == Keys.OemSemicolon && !ShiftDown(modifiers)) { return ';'; }
            if (key == Keys.OemSemicolon && ShiftDown(modifiers)) { return ':'; }
            if (key == Keys.OemTilde && !ShiftDown(modifiers)) { return '`'; }
            if (key == Keys.OemTilde && ShiftDown(modifiers)) { return '~'; }
            if (key == Keys.Subtract) { return '-'; }
            return null;
        }

        public static bool ShiftDown(Modifiers modi)
        {
            return modi == Modifiers.Shift;
        }

        public static bool AltDown(Modifiers modi)
        {
            return modi == Modifiers.Alt;
        }

        public static bool ControlDown(Modifiers modi)
        {
            return modi == Modifiers.Control;
        }
    }

    public class KeyboardEvents
    {
        
        public static int InitialDelay { get; set; }
        public static int RepeatDelay { get; set; }
        
        public KeyboardState previous;
        public Keys lastKey;
        public TimeSpan lastPress;
        public bool isInitial;

        static KeyboardEvents()
        {
            InitialDelay = 800;
            RepeatDelay = 50;
            //KeyTyped += KeyTypedHandler;
        }

        public KeyboardEvents()
        {
            InitialDelay = 800;
            RepeatDelay = 50;
        }

        public static void HandleKeys(GameTime gameTime)
        {
            KeyboardEvents ke = new KeyboardEvents();
            ke.NonStaticHandleKeys(gameTime);
        }

        public void NonStaticHandleKeys(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();

            Modifiers modifiers = Modifiers.None;
            if (currentState.IsKeyDown(Keys.LeftControl) || currentState.IsKeyDown(Keys.RightControl))
            {
                modifiers |= Modifiers.Control;
            }

            if (currentState.IsKeyDown(Keys.LeftShift) || currentState.IsKeyDown(Keys.RightShift))
            {
                modifiers |= Modifiers.Shift;
            }

            if (currentState.IsKeyDown(Keys.LeftAlt) || currentState.IsKeyDown(Keys.RightAlt))
            {
                modifiers |= Modifiers.Alt;
            }

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (currentState.IsKeyDown(key) && previous.IsKeyUp(key))
                { 
                    OnKeyPressed(this, new KeyboardEventArgs(gameTime.TotalGameTime, key,modifiers, currentState));
                    OnKeyTyped(this, new KeyboardEventArgs(gameTime.TotalGameTime,key, modifiers, currentState));

                    lastKey = key;
                    lastPress = gameTime.TotalGameTime;
                    isInitial = true;
                }
            }

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (currentState.IsKeyUp(key) && previous.IsKeyDown(key))
                {
                    OnKeyReleased(this, new KeyboardEventArgs(gameTime.TotalGameTime, key, modifiers, currentState));
                }
            }

            double elapsedTime = (gameTime.TotalGameTime - lastPress).TotalMilliseconds;

            if (currentState.IsKeyDown(lastKey) && ((isInitial && elapsedTime > InitialDelay) || (!isInitial && elapsedTime > RepeatDelay)))
            {
                    OnKeyTyped(this, new KeyboardEventArgs(gameTime.TotalGameTime, lastKey,modifiers,currentState));
                    lastPress = gameTime.TotalGameTime;
                    isInitial = false;
            }

            previous = currentState;
        }

        public void OnKeyPressed(object sender, KeyboardEventArgs args)
        {
            if (KeyPressed != null)
            {
                KeyPressed(sender, args);
            }
        }

        public void OnKeyReleased(object sender, KeyboardEventArgs args)
        {
            if (KeyReleased != null) 
            { 
                KeyReleased(sender, args); 
            }
        }

        public void OnKeyTyped(object sender, KeyboardEventArgs args)
        {
            if (KeyTyped != null)
            {
                KeyTyped(sender, args);
            }
        }

        public static event EventHandler<KeyboardEventArgs> KeyPressed;
        public static event EventHandler<KeyboardEventArgs> KeyReleased;
        public static event EventHandler<KeyboardEventArgs> KeyTyped;

    }

    public class KeyboardEventArgs
    {
        public KeyboardState state {get; protected set;}

        public Modifiers modifiers{get; protected set;}

        public Keys key{get;set;}

        public char? character {get;set;}

        public KeyboardEventArgs(TimeSpan time,Keys key,Modifiers modifiers,KeyboardState currentState)
        {
            this.character = TypingKeyboard.ToChar(key,modifiers);
            this.state = currentState;
            this.modifiers = modifiers;
            this.key = key;
        }
    }
}
