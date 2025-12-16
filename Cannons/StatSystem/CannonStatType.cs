namespace Cannons.StatSystem
{
    public enum CannonStatType
    {
        NONE = 0,
        SHOOT_POWER = 1,      // 발사 속도
        EXPLOSION_DAMAGE = 2, // 폭팔 데미지
        EXPLOSION_RANGE = 3,  // 추가 폭팔 범위
        ADD_COST_SPEED = 4    // 추가되는 코스트 시간 단축
    }
}