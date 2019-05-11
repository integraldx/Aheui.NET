using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AheuiDotnet
{
    enum Direction
    {
        up,
        down,
        left,
        right
    }

    struct ExecutionStatus
    {

    }

    class Processor
    {
        List<List<int>> storageList = new List<List<int>>();
        int xPos = 0;
        int yPos = 0;
        Direction direction = Direction.right;
        int speed = 1;
        Last currentStorageIndicator = Last._;
        CodeArray currentCode = null;

        public Processor()
        {
            for(int i = 0; i < 26; i++)
            {
                storageList.Add(new List<int>());
            }
        }

        public void LoadCode(CodeArray array)
        {
            currentCode = array;
        }

        public bool Step()
        {
            // Console.Error.WriteLine("{0}, {1}", xPos, yPos);
            Token instruction = currentCode.TokenAt(xPos, yPos);

            // Console.Error.WriteLine((char)(0xAC00 + (int)instruction.operation * 21 * 28 + (int)instruction.speed * 28 + (int)instruction.storage));

            bool directionFlipFlag = false;
            
            switch(instruction.operation)
            {
                case First.ㄱ: // No Operation
                case First.ㄲ: // No Operation
                case First.ㅇ: // No Operation
                case First.ㅋ: // No Operation
                case First.ㅉ: // Undefined
                    break;
                case First.ㄷ: // Add
                case First.ㅌ: // Sub
                case First.ㄸ: // Mult
                case First.ㄴ: // Div
                case First.ㄹ: // Mod
                case First.ㅈ: // Comp
                    try
                    {
                        Arithmetic(instruction);
                    }
                    catch (Exception e)
                    {
                        directionFlipFlag = true;
                    }
                    break;
                case First.ㅁ: // Pop
                case First.ㅂ: // Push
                case First.ㅃ: // Clone
                case First.ㅅ: // Select
                case First.ㅆ: // Move
                case First.ㅍ: // Swap
                    try
                    {
                        Storage(instruction);
                    }
                    catch (Exception e)
                    {
                        directionFlipFlag = true;
                    }
                    break;
                case First.ㅊ: // Branch
                    if(Pop(currentStorageIndicator) == 0)
                    {
                        directionFlipFlag = true;
                    }
                    break;
                case First.ㅎ: // Halt/Return
                    speed = 0;
                    break;
            }

            if(directionFlipFlag)
            {
                direction += (int)direction % 2 == 1 ? -1 : 1;
            }

            if (direction == Direction.left || direction == Direction.right)
            {
                xPos = (direction == Direction.left) ? xPos - speed : xPos + speed;
            }
            else if (direction == Direction.up || direction == Direction.down)
            {
                yPos = (direction == Direction.up) ? yPos - speed : yPos + speed;
            }

            if(speed == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void Arithmetic(Token t)
        {
            int valA, valB, valR;
            if(storageList[(int)currentStorageIndicator].Count < 2)
            {
                throw new Exception();
            }
            valA = Pop(currentStorageIndicator);
            valB = Pop(currentStorageIndicator);

            switch(t.operation)
            {
                case First.ㄷ:
                    valR = valB + valA;
                    break;
                case First.ㅌ:
                    valR = valB - valA;
                    break;
                case First.ㄸ:
                    valR = valB * valA;
                    break;
                case First.ㄴ:
                    valR = valB / valA;
                    break;
                case First.ㄹ:
                    valR = valB % valA;
                    break;
                case First.ㅈ: // Comp
                    valR = valB > valA ? 1 : 0;
                    break;
                default:
                    throw new NotImplementedException();
            }

            Push(currentStorageIndicator, valR);
        }

        void Storage(Token t)
        {
            int value;
            switch (t.operation)
            {
                case First.ㅁ: // Pop
                    value = Pop(currentStorageIndicator);
                    if (t.storage == Last.ㅇ)
                    {
                        Console.WriteLine(value);
                    }
                    else if (t.storage == Last.ㅎ)
                    {
                        Console.WriteLine((char)value);
                    }
                    break;
                case First.ㅂ: // Push
                    if (t.storage == Last.ㅇ)
                    {
                        value = int.Parse(Console.ReadLine());
                    }
                    else if (t.storage == Last.ㅎ)
                    {
                        value = Console.Read();
                    }
                    else
                    {
                        value = GetConstant(t.storage);
                    }
                    Push(currentStorageIndicator, value);
                    break;
                case First.ㅃ: // Clone
                    value = Peek(t.storage);
                    storageList[(int)t.storage].Insert(0, value);
                    break;
                case First.ㅅ: // Select
                    currentStorageIndicator = t.storage;
                    break;
                case First.ㅆ: // Move
                    value = Pop(currentStorageIndicator);
                    Push(t.storage, value);
                    break;
                case First.ㅍ: // Swap
                    value = Pop(t.storage);
                    storageList[(int)t.storage].Insert(1, value);
                    break;
            }
        }

        int Pop(Last storage)
        {
            int value;
            if (storage == Last.ㅇ)
            {
                var queue = storageList[(int)storage];
                value = queue[0];
                queue.RemoveAt(0);

            }
            else if (storage == Last.ㅎ)
            {
                throw new NotImplementedException();
            }
            else
            {
                var stack = storageList[(int)storage];
                value = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
            }

            return value;
        }

        void Push(Last storage, int value)
        {
            if (storage == Last.ㅇ)
            {
                var queue = storageList[(int)storage];
                queue.Add(value);
            }
            else if (storage == Last.ㅎ)
            {
                throw new NotImplementedException();
            }
            else
            {
                var stack = storageList[(int)storage];
                stack.Add(value);
            }
        }

        int Peek(Last storage)
        {
            int value;
            if (storage == Last.ㅇ)
            {
                var queue = storageList[(int)storage];
                value = queue[0];

            }
            else if (storage == Last.ㅎ)
            {
                throw new NotImplementedException();
            }
            else
            {
                var stack = storageList[(int)storage];
                value = stack[stack.Count];
            }

            return value;
        }

        static int GetConstant(Last l)
        {
            int value;
            switch(l)
            {
                case Last._:
                    value = 0;
                    break;
                case Last.ㄱ:
                case Last.ㄴ:
                case Last.ㅅ:
                    value = 2;
                    break;
                case Last.ㄷ:
                case Last.ㅈ:
                    value = 3;
                    break;
                case Last.ㅁ:
                case Last.ㅂ:
                case Last.ㅊ:
                case Last.ㅌ:
                case Last.ㅍ:
                case Last.ㄲ:
                case Last.ㄳ:
                case Last.ㅆ:
                    value = 4;
                    break;
                case Last.ㄹ:
                case Last.ㄵ:
                case Last.ㄶ:
                    value = 5;
                    break;
                case Last.ㅄ:
                    value = 6;
                    break;
                case Last.ㄺ:
                case Last.ㄽ:
                    value = 7;
                    break;
                case Last.ㅀ:
                    value = 8;
                    break;
                case Last.ㄻ:
                case Last.ㄼ:
                case Last.ㄾ:
                case Last.ㄿ:
                    value = 9;
                    break;
                default:
                    throw new NotImplementedException(l.ToString());
            }
            return value;
        }
    }
}
