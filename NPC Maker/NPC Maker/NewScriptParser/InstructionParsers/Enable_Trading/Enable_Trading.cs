using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    /*
         * DO_TRADE 
        CORRECT (Correct_message_id)
        WRONG
            MAGICBEAN (Incorrect_message_id_for_magicbean)
            DEFAULT (Default_incorrect_message_id)
        NONE (Talking_message_id)
    END
    
    IF TRADE_STATUS SUCCESS
        instruction
        instruction
        instruction
    ENDIF

    IF TRADE_STATUS WRONG
        instruction
        instruction
        instruction
    ENDIF

    IF TRADE_STATUS NONE
        IF WAS_TALKED_TO
            instruction
            instruction
            instruction
        ELSE
            instruction
            instruction
            instruction    
        ENDIF
    ENDIF*/

    public partial class ScriptParser
    {
        private Instruction ParseEnableTradingInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                UInt16 TextID_Adult = 0;
                UInt16 TextID_Child = 0;

                if (SplitLine.Count() == 2)
                    TextID_Child = TextID_Adult;

                TextID_Adult = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 1, 0, UInt16.MaxValue));
                TextID_Child = (SplitLine.Count() == 2) ? TextID_Adult : Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, TextID_Adult, TextID_Child);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, 0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
