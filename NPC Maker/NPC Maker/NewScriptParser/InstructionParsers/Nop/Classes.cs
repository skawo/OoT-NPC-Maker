using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionNop : Instruction
    {
        public InstructionNop() : base(255)
        {

        }

        public override byte[] ToBytes()
        {
            return new byte[0];
        }
    }
}
