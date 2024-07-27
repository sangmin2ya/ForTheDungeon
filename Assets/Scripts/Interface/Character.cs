using System;
using System.Collections.Generic;

public class Character
{
    //접근 프로퍼티 (읽기 전용)
    public string Name { get; private set; } //이름
    public int Level { get; private set; } //레벨
    public int Experience { get; private set; } //경험치
    public int ExperienceToNextLevel { get; private set; } //다음 레벨까지 필요한 경험치
    public Dictionary<StatType, int> Attributes { get; private set; } //속성과 수치
    public CharacterType Type { get; private set; } //캐릭터 타입
    public AttackType AttackType { get; private set; } //공격타입
    public Tuple<AttackType, int> Shield { get; private set; } //보호막

    //수치 값
    private int _health;
    private int _currentHealth;
    private int _physicalAttack;
    private int _magicAttack;
    private int _evasion;

    //접근 프로퍼티 (읽기 전용)
    public int Health => _health;
    public int CurrentHealth => _currentHealth;
    public int PhysicalAttack => _physicalAttack;
    public int MagicAttack => _magicAttack;
    public int Evasion => _evasion;

    /// <summary>
    /// 플레이어 생성자
    /// </summary>
    /// <param name="type">유저타입 (적, 아군)</param>
    /// <param name="strength"> 힘수치 </param>
    /// <param name="vitality">활력</param>
    /// <param name="intelligence">지능</param>
    /// <param name="vision">인지</param>
    /// <param name="speed">속도</param>
    public Character(CharacterType type, string name, int level, int strength, int vitality, int intelligence, int vision, int speed, Tuple<AttackType, int> shield, AttackType attackType)
    {
        Type = type;
        Name = name;
        Level = level;
        Experience = 0;
        ExperienceToNextLevel = 100;
        Shield = shield;
        Attributes = new Dictionary<StatType, int>
        {
            { StatType.Strength, strength },
            { StatType.Vitality, vitality },
            { StatType.Intelligence, intelligence },
            { StatType.Vision, vision },
            { StatType.Speed, speed }
        };
        for (int i = 0; i < Level; i++)
        {
            LevelUpAttributes();
        }
        UpdateStats();
        _currentHealth = _health;
    }
    /// <summary>
    /// 경험치 획득, 외부에서는 해당 메소드로만 접근 가능
    /// </summary>
    /// <param name="amount">경험치양</param>
    public void GainExperience(int amount)
    {
        Experience += amount;
        while (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }
    public void UseLevelUp()
    {
        Level++;
        Experience -= ExperienceToNextLevel;
        //레벨업 필요 경험치 증가
        ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.3);
        LevelUpAttributes();
        UpdateStats();
    }
    /// <summary>
    /// 데미지 입음
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        if (_currentHealth - amount <= 0)
        {
            _currentHealth = 0;
            return;
        }
        _currentHealth -= amount;
    }
    /// <summary>
    /// 회복
    /// </summary>
    /// <param name="amount">회복량</param>
    public void Heal(int amount)
    {
        if (_currentHealth + amount > _health)
        {
            _currentHealth = _health;
            return;
        }
        _currentHealth += amount;
    }
    /// <summary>
    /// 스텟 영구증가
    /// </summary>
    /// <param name="statType">증가시키고 싶은 스텟</param>
    /// <param name="amount">증가 스텟 양</param>
    public void IncreaseStat(StatType statType, int amount)
    {
        Attributes[statType] += amount;
        UpdateStats();
    }
    public void decreaseStat(StatType statType, int amount)
    {
        Attributes[statType] -= amount;
        UpdateStats();
    }
    /// <summary>
    /// 레벨업 시 레벨 증가, 경험치 감소
    /// </summary>
    private void LevelUp()
    {
        Level++;
        Experience -= ExperienceToNextLevel;
        //레벨업 필요 경험치 증가
        ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.3);
        LevelUpAttributes();
        UpdateStats();
    }
    /// <summary>
    /// 레벨업 시 스텟 전부 +1
    /// </summary>
    private void LevelUpAttributes()
    {
        IncreaseStats();
        UpdateStats();
    }
    private void IncreaseStats()
    {
        // 스탯 증가량 (레벨당)
        int vitalityIncrease = 3;
        int strengthIncrease = 2;
        int intelligenceIncrease = 3;
        int speedIncrease = 1;

        // 스탯 증가
        Attributes[StatType.Vitality] += vitalityIncrease;
        Attributes[StatType.Strength] += strengthIncrease;
        Attributes[StatType.Intelligence] += intelligenceIncrease;
        Attributes[StatType.Speed] += speedIncrease;
    }
    /// <summary>
    /// 스텟 업데이트
    /// </summary>
    private void UpdateStats()
    {
        _health = Attributes[StatType.Vitality] * 3;
        _physicalAttack = Attributes[StatType.Strength] * 2;
        _magicAttack = Attributes[StatType.Intelligence] * 2;
        _evasion = Attributes[StatType.Speed] * 1;
    }
    /// <summary>
    /// 캐릭터 정보 출력
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"Type: {Type}, Level: {Level}, Experience: {Experience}/{ExperienceToNextLevel}, Health: {CurrentHealth}/{Health}, Physical Attack: {PhysicalAttack}, Magic Attack: {MagicAttack}, Evasion: {Evasion}, Attributes: {string.Join(", ", Attributes)}";
    }
}