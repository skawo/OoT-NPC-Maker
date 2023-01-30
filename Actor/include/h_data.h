#ifndef H_DATA_H
#define H_DATA_H

#include "npc_maker_types.h"

ExSegDataEntry* Data_GetExtraSegmentData(NpcMaker* en, int segment_index, int entry);
InternalMsgEntry Data_GetCustomMessage(NpcMaker* en, PlayState* playState, int ID);

#endif 