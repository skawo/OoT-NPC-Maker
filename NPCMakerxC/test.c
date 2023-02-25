#include <z64hdr/oot_mq_debug/z64hdr.h>


void Function2(Actor* thisx, PlayState* playState)
{
		Vec3f pos = {thisx->world.pos.x, thisx->world.pos.y, thisx->world.pos.z};
	
	EffectSsIcePiece_SpawnBurst(playState, &pos, 100); 
}

void Function(Actor* thisx, PlayState* playState)
{
	gSaveContext.rupees++;
	Function2(thisx, playState);

}

void* FunctionList[] = 
{
    (void*)0xDEADBEEF,
	(void*)&Function,
	(void*)0xDEADBEEF
};