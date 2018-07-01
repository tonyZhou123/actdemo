//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Framework;

//namespace KGame
//{
//    /// <summary>
//    /// 负责角色、NPC、Monster的动画播放（包括技能、移动等）、动画资源的Override
//    /// 分三部分动画：通用、普攻、技能
//    /// </summary>
//    public class AnimationController : MonoBehaviour
//    {
//        public enum SkillAction
//        {
//            SA_StartAction,             // 起手动画
//            SA_Sing,                    // 吟唱动画
//            SA_Guide,                   // 引导动画
//            SA_FinishSing,              // 吟唱正常结束
//            SA_FinishGuide,             // 引导正常结束
//        }

//        // 动画


//        public struct BackupParam
//        {
//            public AnimatorControllerParameter animatorParam;
//            public object                      value;
//        }

//        private Animator                   _animator;
//        private UsualClipParam             _playerUsualClipParam = new UsualClipParam();
//        private UsualClipParam             _weaponUsualClipParam = new UsualClipParam();


//        private AnimatorOverrideController _overrideController;
//        private List<BackupParam>          _params = new List<BackupParam>();
//        private SimpleAnimation            _weaponAnimation;
//        private RuntimeAnimatorController  _cachedController;
//        private Vector3                    _lookAtPosition;
//        private bool                       _ikActive;

//        public Animator                    animator
//        {
//            get { return _animator; }
//        }

//        public AnimatorOverrideController  animatorOverrideController
//        {
//            get
//            {
//                if (_animator != null && _overrideController == null)
//                {
//                    RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
//                    if (controller is AnimatorOverrideController)
//                    {
//                        _overrideController = (AnimatorOverrideController)controller;
//                    }
//                    else
//                    {
//                        if (controller != null)
//                        {
//                            _overrideController = new AnimatorOverrideController();
//                            _overrideController.runtimeAnimatorController = controller;
//                            _animator.runtimeAnimatorController = _overrideController;
//                        }     
//                    }
//                }
//                return _overrideController;
//            }
//        }

//        ///  ///////////////////////////////////// 动画资源名，动态替换资源时使用
//        static public string idle                       = "idle";
//        static public string battle_idle                = "battle_idle";
//        static public string walk                       = "walk";
//        static public string run                        = "run";
//        static public string escape                     = "escape";
//        static public string dizzy                      = "dizzy";
//        static public string beHit                      = "behit";
//        static public string dead                       = "dead";
//        static public string jump_start                 = "jump_start";
//        static public string jump_loop                  = "jump_loop";
//        static public string land                       = "land";
//        static public string fall                       = "fall";
//        static public string customize                  = "customize";
//        static public string loopCustomize              = "loopCustomize";

//        static public string beginmount                 = "beginmount";
//        static public string leavemount                 = "leavemount";

//        static public string riderun                    = "riderun";
//        static public string rideidle                   = "rideidle";
//        static public string rideflycharge              = "rideflysprint";
//        static public string rideflyidle                = "rideflyidle";
//        static public string rideflyrun                 = "rideflyrun";
//        static public string rideflyup                  = "rideflyup";
//        static public string rideflydown                = "rideflydown";

//        // 普通攻击
//        static public string baseAttack0                = "baseAttack0";
//        static public string baseAttack1                = "baseAttack1";
//        static public string baseAttack2                = "baseAttack2";
//        static public string baseAttack3                = "baseAttack3";
//        static public string baseAttack4                = "baseAttack4";

//        // skill0
//        static public string skill0_start               = "skill0_start";
//        static public string skill0_guide               = "skill0_guide";
//        static public string skill0_sing                = "skill0_sing";
//        static public string skill0_cast                = "skill0_cast";
        
//        // skill1
//        static public string skill1_start               = "skill1_start";
//        static public string skill1_guide               = "skill1_guide";
//        static public string skill1_sing                = "skill1_sing";
//        static public string skill1_cast                = "skill1_cast";

//        // skill2
//        static public string skill2_start               = "skill2_start";
//        static public string skill2_guide               = "skill2_guide";
//        static public string skill2_sing                = "skill2_sing";
//        static public string skill2_cast                = "skill2_cast";

//        // skill3
//        static public string skill3_start               = "skill3_start";
//        static public string skill3_guide               = "skill3_guide";
//        static public string skill3_sing                = "skill3_sing";
//        static public string skill3_cast                = "skill3_cast";

//        // skill5  fulu
//        static public string skill5_start               = "skill5_start";
//        static public string skill5_guide               = "skill5_guide";
//        static public string skill5_sing                = "skill5_sing";
//        static public string skill5_cast                = "skill5_cast";

//        /// /////////////////////////////////////// hash of parameters
//        static public int _playbackHash                 = Animator.StringToHash("playback");
//        static public int _movementHash                 = Animator.StringToHash("movement");
//        static public int _idleHash                     = Animator.StringToHash("bIdle");
//        static public int _walkHash                     = Animator.StringToHash("bWalk");
//        static public int _doInterruptSkillHash         = Animator.StringToHash("doInterruptSkill");
//        static public int _doCustomizeHash              = Animator.StringToHash("doCustomize");
//        static public int _doLoopCustomizeHash          = Animator.StringToHash("doLoopCustomize");                 // 循环自定义动画
       
//        static public int _doDeadHash                   = Animator.StringToHash("doDead");     
//        static public int _doBehitHash                  = Animator.StringToHash("doBehit");
//        static public int _doJumpHash                   = Animator.StringToHash("doJump");
//        static public int _doFinishJumpHash             = Animator.StringToHash("doFinishJump");
//        static public int _doFallHash                   = Animator.StringToHash("doFall");
     
//        static public int _doBeginMountHash             = Animator.StringToHash("doBeginMount");
//        static public int _doLeaveMountHash             = Animator.StringToHash("doLeaveMount");

//        // base attack
//        static public int _doBaseAttack0Hash            = Animator.StringToHash("doBaseAttack0");
//        static public int _doBaseAttack1Hash            = Animator.StringToHash("doBaseAttack1");
//        static public int _doBaseAttack2Hash            = Animator.StringToHash("doBaseAttack2");
//        static public int _doBaseAttack3Hash            = Animator.StringToHash("doBaseAttack3");
//        static public int _doBaseAttack4Hash            = Animator.StringToHash("doBaseAttack4");

//        // skill0
//        static public int _hash_skill0_doStartAction    = Animator.StringToHash("skill0_doStartAction");
//        static public int _hash_skill0_doSing           = Animator.StringToHash("skill0_doSing");
//        static public int _hash_skill0_doGuide          = Animator.StringToHash("skill0_doGuide");
//        static public int _hash_skill0_doFinishSing     = Animator.StringToHash("skill0_doFinishSing");
//        static public int _hash_skill0_doFinishGuide    = Animator.StringToHash("skill0_doFinishGuide");

//        // skill1
//        static public int _hash_skill1_doStartAction    = Animator.StringToHash("skill1_doStartAction");
//        static public int _hash_skill1_doSing           = Animator.StringToHash("skill1_doSing");
//        static public int _hash_skill1_doGuide          = Animator.StringToHash("skill1_doGuide");
//        static public int _hash_skill1_doFinishSing     = Animator.StringToHash("skill1_doFinishSing");
//        static public int _hash_skill1_doFinishGuide    = Animator.StringToHash("skill1_doFinishGuide");

//        // skill2
//        static public int _hash_skill2_doStartAction    = Animator.StringToHash("skill2_doStartAction");
//        static public int _hash_skill2_doSing           = Animator.StringToHash("skill2_doSing");
//        static public int _hash_skill2_doGuide          = Animator.StringToHash("skill2_doGuide");
//        static public int _hash_skill2_doFinishSing     = Animator.StringToHash("skill2_doFinishSing");
//        static public int _hash_skill2_doFinishGuide    = Animator.StringToHash("skill2_doFinishGuide");

//        // skill3
//        static public int _hash_skill3_doStartAction    = Animator.StringToHash("skill3_doStartAction");
//        static public int _hash_skill3_doSing           = Animator.StringToHash("skill3_doSing");
//        static public int _hash_skill3_doGuide          = Animator.StringToHash("skill3_doGuide");
//        static public int _hash_skill3_doFinishSing     = Animator.StringToHash("skill3_doFinishSing");
//        static public int _hash_skill3_doFinishGuide    = Animator.StringToHash("skill3_doFinishGuide");

//        // skill5
//        static public int _hash_skill5_doStartAction    = Animator.StringToHash("skill5_doStartAction");
//        static public int _hash_skill5_doSing           = Animator.StringToHash("skill5_doSing");
//        static public int _hash_skill5_doGuide          = Animator.StringToHash("skill5_doGuide");
//        static public int _hash_skill5_doFinishSing     = Animator.StringToHash("skill5_doFinishSing");
//        static public int _hash_skill5_doFinishGuide    = Animator.StringToHash("skill5_doFinishGuide");

//        static public int _doMount                      = Animator.StringToHash("doMount");
//        static public int _doFlyMoveY                   = Animator.StringToHash("doFlyMoveY");
//        static public int _doFlySprint                  = Animator.StringToHash("doFlySprint");

//        void Awake()
//        {
//            _animator = GetComponentInChildren<Animator>();
//        }

//        void OnDestroy()
//        {
//            UnloadUsualClips();
//        }

//        void Reset()
//        {
//            if (_animator != null )
//            {
//                _animator.Rebind();            
//                RestoreParams();
//            }
//        }

//        void BackupParams()
//        {
//            if (_animator != null && _animator.isInitialized)
//            {
//                _params.Clear();

//                BackupParam param = new BackupParam();

//                param.animatorParam = _animator.GetParameter(0);            // playback
//                param.value = _animator.GetFloat(_playbackHash);
//                _params.Add(param);

//                param = new BackupParam();
//                param.animatorParam = _animator.GetParameter(1);            // movement
//                param.value = _animator.GetFloat(_movementHash);
//                _params.Add(param);

//                param = new BackupParam();                                  // bIdle
//                param.animatorParam = _animator.GetParameter(2);
//                param.value = _animator.GetFloat(_idleHash);
//                _params.Add(param);

//                param = new BackupParam();                                  // bWalk
//                param.animatorParam = _animator.GetParameter(3);
//                param.value = _animator.GetFloat(_walkHash);
//                _params.Add(param);

//                param = new BackupParam();                                  // bWalk
//                param.animatorParam = _animator.GetParameter(4);
//                param.value = _animator.GetInteger(_doMount);
//                _params.Add(param);
//            }
//        }

//        void RestoreParams()
//        {
//            if (_animator != null && _animator.isInitialized)
//            {
//                for (int i = 0; i < _params.Count; ++i)
//                {
//                    BackupParam param = _params[i];
//                    switch (param.animatorParam.type)
//                    {
//                        case AnimatorControllerParameterType.Bool:
//                            _animator.SetBool(param.animatorParam.nameHash, (bool)param.value);
//                            break;
//                        case AnimatorControllerParameterType.Float:
//                            _animator.SetFloat(param.animatorParam.nameHash, (float)param.value);
//                            break;
//                        case AnimatorControllerParameterType.Int:
//                            _animator.SetInteger(param.animatorParam.nameHash, (int)param.value);
//                            break;
//                    }
//                }
//            }
//        }
        
//        /// <summary>
//        /// 设置动画
//        /// </summary>
//        /// <param name="param"></param>
//        public void SetUsualClipParams(Dictionary<string, string> animDict)
//        {
//            if (_animator == null)
//                return;
//            foreach (var current in animDict)
//            {
//                ChangeClip(current.Key, current.Value);
//            }
//        }

//        /// <summary>
//        /// 卸载动画
//        /// </summary>
//        public void UnloadUsualClips()
//        {
//            if (_playerUsualClipParam != null)
//            {
//                List<string> keys = new List<string>(_playerUsualClipParam.dict.Keys);
//                for (int i = 0; i < keys.Count; i++)
//                {
//                    ChangeClip(keys[i], null);
//                }
//                _playerUsualClipParam = null;
//            }
//            if (_weaponUsualClipParam != null)
//            {
//                List<string> keys = new List<string>(_weaponUsualClipParam.dict.Keys);
//                for (int i = 0; i < keys.Count; i++)
//                {
//                    ChangeWeaponClip(keys[i], null);
//                }
//                _weaponUsualClipParam = null;
//            }
//        }

//        public void ApplyRootMotion(bool enabled)
//        {
//            if (_animator != null)
//            {
//                _animator.applyRootMotion = enabled;
//            }
//        }

//        public void SetPlayback(float speed)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetFloat(_playbackHash, speed);
//        }

//        // 0: 停止
//        // 1: 移动
//        public void SetMovement(float value, float dampTime, float deltaTime)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetFloat(_movementHash, value, dampTime, deltaTime);
//        }

//        // 设置移动动画（walk or run or escape）
//        // 0.0: walk
//        // 0.5: run
//        // 1.0: escape
//        public void PlayWalk(float value)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetFloat(_walkHash, value);
//        }

//        // 设置Idle or battleIdle
//        public void PlayIdle(bool bIdle)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetFloat(_idleHash, bIdle ? 0 : 1);
//        }

//        public void SetIdleClip(string assetPath)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            ChangeClip(idle, assetPath);
//        }

//        public void SetWalkClip(string assetPath)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            ChangeClip(walk, assetPath);
//        }

//        public void SetRunClip(string assetPath)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            ChangeClip(run, assetPath);
//        }

//        public void PlayCustomizedWeaponAnimation(string weaponAnimAssetPath)
//        {
//            if (_weaponAnimation == null)
//            {
//                return;
//            }
//            if (string.IsNullOrEmpty(weaponAnimAssetPath))
//            {
//                return;
//            }
//            AssetLoader<AnimationClip> assetLoader = ChangeWeaponClip(customize, weaponAnimAssetPath);
//            if (assetLoader != null && assetLoader.asset != null)
//            {
//                assetLoader.asset.wrapMode = WrapMode.Once;
//            }
//            PlayWeaponAnimation(customize);
//        }

//        // 设置自定义动画资源，并且播放
//        public void PlayCustomizedAnimation(string playerAnimAssetPath)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            if (string.IsNullOrEmpty(playerAnimAssetPath))
//            {
//                return;
//            }
//            AssetLoader<AnimationClip> assetLoader = ChangeClip(customize, playerAnimAssetPath);
//            if (assetLoader != null && assetLoader.asset != null)
//            {
//                assetLoader.asset.wrapMode = WrapMode.Once;
//            }
//            _animator.SetTrigger(_doCustomizeHash);
//            _animator.Play(customize, 0, 0);
//        }

//        public void PlayLoopCustomizedAnimation(string playerAnimAssetPath)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            if (string.IsNullOrEmpty(playerAnimAssetPath))
//                return;
//            AssetLoader<AnimationClip> assetLoader = ChangeClip(loopCustomize, playerAnimAssetPath);
//            if (assetLoader != null && assetLoader.asset != null)
//            {
//                assetLoader.asset.wrapMode = WrapMode.Loop;
//            }
//            _animator.SetBool(_doLoopCustomizeHash, true);
//            _animator.Play("loop customize", 0, 0);
//        }

//        public void FinishCustomizedAnimation()
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.ResetTrigger(_doCustomizeHash);
//        }

//        public void FinishLoopCustomizedAnimation()
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetBool(_doLoopCustomizeHash, false);
//        }

//        // 中断当前动画
//        public void ForceToIdle()
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetTrigger(_doInterruptSkillHash);
//        }

//        public void PlayBehit()
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetTrigger(_doBehitHash);
//        }

//        public void SetDead(bool dead, float normalizedTime = 0f)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetBool(_doDeadHash, dead);
//            _animator.Play("Dead", 0, normalizedTime);
//        }

        
//        public void PlayJump(bool jump)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetBool(_doJumpHash, jump);
//        }

//        public void SetFall(bool active)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            _animator.SetBool(_doFallHash, active);
//        }

//        /// <summary>
//        /// 设置普攻动画
//        /// </summary>
//        /// <param name="param"></param>
//        public void SetBaseAttackParams(Dictionary<string, string> playerBaseAttackAnimDict, Dictionary<string, string> weaponBaseAttackAnimDict)
//        {
//            if (_animator == null)
//                return;
//            InitWeaponAnimationComponent();
//            foreach (var s in playerBaseAttackAnimDict)
//            {
//                ChangeClip(s.Key, s.Value, true);
//            }
//            foreach (var s in weaponBaseAttackAnimDict)
//            {
//                ChangeWeaponClip(s.Key, s.Value, true);
//            }
//        }
        
//        /// <summary>
//        /// 播放普攻动画
//        /// </summary>
//        /// <param name="phase"></param>
//        public void PlayBaseAttackAnimation(int phase)
//        {
//            if (_animator == null)
//                return;

//            switch (phase)
//            {
//                case 0:
//                    _animator.SetTrigger(_doBaseAttack0Hash);
//                    PlayWeaponAnimation(baseAttack0);
//                    break;
//                case 1:
//                    _animator.SetTrigger(_doBaseAttack1Hash);
//                    PlayWeaponAnimation(baseAttack1);
//                    break;
//                case 2:
//                    _animator.SetTrigger(_doBaseAttack2Hash);
//                    PlayWeaponAnimation(baseAttack2);
//                    break;
//                case 3:
//                    _animator.SetTrigger(_doBaseAttack3Hash);
//                    PlayWeaponAnimation(baseAttack3);
//                    break;
//                case 4:
//                    _animator.SetTrigger(_doBaseAttack4Hash);
//                    PlayWeaponAnimation(baseAttack4);
//                    break;
//            }
//        }
        
//        /// <summary>
//        /// 设置技能动画
//        /// </summary>
//        /// <param name="slot"></param>
//        /// <param name="param"></param>
//        public void SetSkillParams(Dictionary<string, string> playerSkillAnimDict, Dictionary<string, string> weaponSkillAnimDict)
//        {
//            if (_animator == null)
//                return;
//            InitWeaponAnimationComponent();

//            foreach (var s in playerSkillAnimDict)
//            {
//                ChangeClip(s.Key, s.Value, true);
//            }
//            foreach (var s in weaponSkillAnimDict)
//            {
//                ChangeWeaponClip(s.Key, s.Value, true);
//            }
//        }
        
//        /// <summary>
//        /// 播放技能动画
//        /// </summary>
//        /// <param name="slot"></param>
//        /// <param name="action"></param>
//        public void PlaySkillAnimation(int slot, SkillAction action)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            switch (slot)
//            {
//                case 0:
//                    switch (action)
//                    {
//                        case SkillAction.SA_StartAction:
//                            _animator.SetTrigger(_hash_skill0_doStartAction);
//                            PlayWeaponAnimation(skill0_start);
//                            break;
//                        case SkillAction.SA_Sing:
//                            _animator.SetTrigger(_hash_skill0_doSing);
//                            PlayWeaponAnimation(skill0_sing);
//                            break;
//                        case SkillAction.SA_Guide:
//                            _animator.SetTrigger(_hash_skill0_doGuide);
//                            PlayWeaponAnimation(skill0_guide);
//                            break;
//                        case SkillAction.SA_FinishGuide:
//                            _animator.SetTrigger(_hash_skill0_doFinishGuide);
//                            PlayWeaponAnimation(null);
//                            break;
//                        case SkillAction.SA_FinishSing:
//                            _animator.SetTrigger(_hash_skill0_doFinishSing);
//                            PlayWeaponAnimation(skill0_cast);
//                            break;
//                    }
//                    break;
//                case 1:
//                    switch (action)
//                    {
//                        case SkillAction.SA_StartAction:
//                            _animator.SetTrigger(_hash_skill1_doStartAction);
//                            PlayWeaponAnimation(skill1_start);
//                            break;
//                        case SkillAction.SA_Sing:
//                            _animator.SetTrigger(_hash_skill1_doSing);
//                            PlayWeaponAnimation(skill1_sing);
//                            break;
//                        case SkillAction.SA_Guide:
//                            _animator.SetTrigger(_hash_skill1_doGuide);
//                            PlayWeaponAnimation(skill1_guide);
//                            break;
//                        case SkillAction.SA_FinishGuide:
//                            _animator.SetTrigger(_hash_skill1_doFinishGuide);
//                            PlayWeaponAnimation(null);
//                            break;
//                        case SkillAction.SA_FinishSing:
//                            _animator.SetTrigger(_hash_skill1_doFinishSing);
//                            PlayWeaponAnimation(skill1_cast);
//                            break;
//                    }
//                    break;
//                case 2:
//                    switch (action)
//                    {
//                        case SkillAction.SA_StartAction:
//                            _animator.SetTrigger(_hash_skill2_doStartAction);
//                            PlayWeaponAnimation(skill2_start);
//                            break;
//                        case SkillAction.SA_Sing:
//                            _animator.SetTrigger(_hash_skill2_doSing);
//                            PlayWeaponAnimation(skill2_sing);
//                            break;
//                        case SkillAction.SA_Guide:
//                            _animator.SetTrigger(_hash_skill2_doGuide);
//                            PlayWeaponAnimation(skill2_guide);
//                            break;
//                        case SkillAction.SA_FinishGuide:
//                            _animator.SetTrigger(_hash_skill2_doFinishGuide);
//                            PlayWeaponAnimation(null);
//                            break;
//                        case SkillAction.SA_FinishSing:
//                            _animator.SetTrigger(_hash_skill2_doFinishSing);
//                            PlayWeaponAnimation(skill2_cast);
//                            break;
//                    }
//                    break;
//                case 3:

//                    switch (action)
//                    {
//                        case SkillAction.SA_StartAction:
//                            _animator.SetTrigger(_hash_skill3_doStartAction);
//                            PlayWeaponAnimation(skill3_start);
//                            break;
//                        case SkillAction.SA_Sing:
//                            _animator.SetTrigger(_hash_skill3_doSing);
//                            PlayWeaponAnimation(skill3_sing);
//                            break;
//                        case SkillAction.SA_Guide:
//                            _animator.SetTrigger(_hash_skill3_doGuide);
//                            PlayWeaponAnimation(skill3_guide);
//                            break;
//                        case SkillAction.SA_FinishGuide:
//                            _animator.SetTrigger(_hash_skill3_doFinishGuide);
//                            PlayWeaponAnimation(null);
//                            break;
//                        case SkillAction.SA_FinishSing:
//                            _animator.SetTrigger(_hash_skill3_doFinishSing);
//                            PlayWeaponAnimation(skill3_cast);
//                            break;
//                    }
//                    break;
//                case 5:
//                    switch (action)
//                    {
//                        case SkillAction.SA_StartAction:
//                            _animator.SetTrigger(_hash_skill5_doStartAction);
//                            PlayWeaponAnimation(skill5_start);
//                            break;
//                        case SkillAction.SA_Sing:
//                            _animator.SetTrigger(_hash_skill5_doSing);
//                            PlayWeaponAnimation(skill5_sing);
//                            break;
//                        case SkillAction.SA_Guide:
//                            _animator.SetTrigger(_hash_skill5_doGuide);
//                            PlayWeaponAnimation(skill5_guide);
//                            break;
//                        case SkillAction.SA_FinishGuide:
//                            _animator.SetTrigger(_hash_skill5_doFinishGuide);
//                            PlayWeaponAnimation(null);
//                            break;
//                        case SkillAction.SA_FinishSing:
//                            _animator.SetTrigger(_hash_skill5_doFinishSing);
//                            PlayWeaponAnimation(skill5_cast);
//                            break;
//                    }
//                    break;
//            }
//        }

//        public void PlayWeaponAnimation(string stateName)
//        {
//            if (_weaponAnimation == null)
//                return;
//            _weaponAnimation.Play(stateName);
//        }


//        public void SetActive(bool bEnable)
//        {
//            if (!bEnable)
//            {
//                _cachedController = _animator.runtimeAnimatorController;
//                BackupParams();
//                _animator.runtimeAnimatorController = null;
//            }
//            else
//            {
//                _animator.runtimeAnimatorController = _cachedController;
//                Reset();
//            }
//        }

//        public void PlayMountType(int mountType)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            this.animator.SetInteger(_doMount, mountType);
//        }

//        private System.Action _beginMountFinish;
//        private System.Action _leaveMountFinish;

//        void Update()
//        {
//            if (_animator == null)
//            {
//                return;
//            }
//            if (_animator.runtimeAnimatorController == null)
//            {
//                return;
//            }
//            AnimatorStateInfo baseAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
//            if (baseAnimatorStateInfo.IsName(beginmount) && baseAnimatorStateInfo.normalizedTime >= 0.9f)
//            {
//                if (_beginMountFinish != null)
//                {
//                    _beginMountFinish();
//                }
//                _beginMountFinish = null;
//            }
//            if (baseAnimatorStateInfo.IsName(leavemount) && baseAnimatorStateInfo.normalizedTime >= 0.9f)
//            {
//                if (_leaveMountFinish != null)
//                {
//                    _leaveMountFinish();
//                }
//                _leaveMountFinish = null;
//            }
//        }

//        public void PlayBeginMount(string path, System.Action beginMountFinish)
//        {
//            ChangeClip(beginmount, path);
//            if (_animator != null && _animator.runtimeAnimatorController != null)
//            {
//                _beginMountFinish = beginMountFinish;
//                _animator.SetTrigger(_doBeginMountHash);
//            }
//        }

//        public void PlayLeaveMount(string path, System.Action leaveMountFinish)
//        {
//            ChangeClip(leavemount, path);
//            if (_animator != null && _animator.runtimeAnimatorController != null)
//            {
//                _leaveMountFinish = leaveMountFinish;
//                _animator.SetTrigger(_doLeaveMountHash);
//            }
//        }

//        public void PlayFlyUpOrDown(float flyMoveY, float dampTime, float deltaTime)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            this.animator.SetFloat(_doFlyMoveY, flyMoveY, dampTime, deltaTime);
//        }

//        public void PlayFlySprint(float flySprint, float dampTime, float deltaTime)
//        {
//            if (_animator == null)
//                return;
//            if (_animator.runtimeAnimatorController == null)
//                return;
//            this.animator.SetFloat(_doFlySprint, flySprint, dampTime, deltaTime);
//        }

//        public void PlayLookAtPosition(Vector3 lookAtPosition)
//        {
//            this._lookAtPosition = lookAtPosition;
//            this._ikActive = true;
//        }

//        public void StopLookAtIK()
//        {
//            this._ikActive = false;
//        }

//        public bool IKActive()
//        {
//            return this._ikActive;
//        }

//        void OnAnimatorIK(int layerIndex)
//        {
//            if (_animator == null)
//                return;
//            if (layerIndex == 0 && _ikActive)
//            {
//                animator.SetLookAtWeight(1.0f);
//                animator.SetLookAtPosition(_lookAtPosition);
//            }
//        }

//        // 初始化武器动画组件
//        void InitWeaponAnimationComponent()
//        {
//            if (_weaponAnimation == null)
//            {
//                PlayerController pc = GetComponentInParent<PlayerController>();
//                if (pc != null && pc.Weapon1 != null)
//                {
//                    _weaponAnimation = pc.Weapon1.gameObject.GET<SimpleAnimation>();
//                }
//            }
//        }

//        // 按动画槽和路径替换动画
//        AssetLoader<AnimationClip> ChangeClip(string animName, string animNewPath, bool forceReplace = false)
//        {
//            if (_animator == null)
//                return null;
//            UsualClipData data = null;
//            _playerUsualClipParam.dict.TryGetValue(animName, out data);

//            if (data != null)
//            {
//                if (data.animAsset != null)
//                {
//                    if (data.animPath == animNewPath && forceReplace == false)
//                    {
//                        return data.animAsset;
//                    }
//                }
//            }
//            else
//            {
//                data = _playerUsualClipParam.AddAnim(animName, animNewPath);
//            }
//            if (data.animAsset != null)
//            {
//                data.animAsset.Unload();
//                data.animAsset = null;
//            }
//            if (string.IsNullOrEmpty(animNewPath) == false)
//            {
//                data.animAsset = new AssetLoader<AnimationClip>(animNewPath);
//            }
//            data.animPath = animNewPath;
//            if (animatorOverrideController != null)
//            {
//                animatorOverrideController[animName] = data.animAsset != null ? data.animAsset.asset : null;
//            }
//            return data.animAsset;
//        }

//        // 按动画槽和路径替换武器动画
//        AssetLoader<AnimationClip> ChangeWeaponClip(string animName, string animNewPath, bool forceReplace = false)
//        {
//            if (_weaponAnimation == null)
//                return null;
//            UsualClipData data = null;
//            _weaponUsualClipParam.dict.TryGetValue(animName, out data);

//            if (data != null)
//            {
//                if (data.animAsset != null)
//                {
//                    if (data.animPath == animNewPath && forceReplace == false)
//                    {
//                        return data.animAsset;
//                    }
//                }
//            }
//            else
//            {
//                data = _weaponUsualClipParam.AddAnim(animName, animNewPath);
//            }
//            if (data.animAsset != null)
//            {
//                data.animAsset.Unload();
//                data.animAsset = null;
//            }
//            if (string.IsNullOrEmpty(animNewPath) == false)
//            {
//                data.animAsset = new AssetLoader<AnimationClip>(animNewPath);
//            }
//            data.animPath = animNewPath;
//            _weaponAnimation.AddClip(data.animAsset != null ? data.animAsset.asset : null, animName);
//            return data.animAsset;
//        }

//    }
//}