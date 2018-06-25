
/// <summary>
/// eventCenter的ID，每个id都不能重复，不然会乱
/// </summary>
public class EventID
{
    public const int ChangeCharacterSpeed = 8000;
    public const int ChangeCharacterCool = 8001;

    public const int LoadPanel_SetSlider = 9000;
    public const int LoadPanel_SetImage = 9001;

    public const int NoticeEvent = 10000;

    public const int UIMain_RefreshAttack = 11000;
    public const int UIMain_RefreshDefence = 11001;
    public const int UIMain_RefreshSkill1 = 11002;
    public const int UIMain_RefreshSkill2 = 11003;
    public const int UIMain_ShowEnemyDamage = 11004;
    public const int UIMain_ShowPlayerDamage = 11005;
    public const int UIMain_RefreshHealthPoint = 11006;
    public const int UIMain_RefreshMonsterPos = 11007;

    public const int Setting1_RefreshAbout = 12000;

    public const int MonsterAI_AITreeActive = 13000;
    public const int MonsterAI_CreateDeathEffect = 13001;

    public const int LoginPanel_Refresh = 14000;
}
