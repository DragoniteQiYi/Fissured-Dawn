using System;
using System.Collections.Generic;

namespace FissuredDawn.Global.Interfaces.GameServices
{
    public interface IGameVariableService
    {
        // ========== 布尔标志操作 ==========

        /// <summary>
        /// 获取一个布尔值变量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool GetFlag(string key, bool defaultValue = false);

        /// <summary>
        /// 获取一个布尔值变量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool SetFlag(string key, bool value);

        /// <summary>
        /// 切换布尔标志值（如果不存在则设置为true）
        /// </summary>
        /// <param name="key">标志键名</param>
        /// <returns>切换后的值</returns>
        bool ToggleFlag(string key);

        /// <summary>
        /// 检查布尔标志是否存在
        /// </summary>
        bool HasFlag(string key);

        // ========== 变量操作 ==========

        /// <summary>
        /// 获取变量值（强类型）
        /// </summary>
        /// <typeparam name="T">期望的类型（int, float, string, bool）</typeparam>
        /// <param name="key">变量键名</param>
        /// <param name="defaultValue">默认值（当键不存在时返回）</param>
        /// <returns>变量值</returns>
        T GetVariable<T>(string key, T defaultValue = default);

        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <typeparam name="T">变量类型</typeparam>
        /// <param name="key">变量键名</param>
        /// <param name="value">要设置的值</param>
        void SetVariable<T>(string key, T value);

        /// <summary>
        /// 检查变量是否存在
        /// </summary>
        bool HasVariable(string key);

        /// <summary>
        /// 获取集合变量
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="key">集合键名</param>
        /// <returns>集合副本</returns>
        List<T> GetVariableSet<T>(string key);

        /// <summary>
        /// 设置集合变量
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="key">集合键名</param>
        /// <param name="values">集合值</param>
        void SetVariableSet<T>(string key, IEnumerable<T> values);

        /// <summary>
        /// 向集合添加元素
        /// </summary>
        void AddToVariableSet<T>(string key, T item);

        /// <summary>
        /// 从集合移除元素
        /// </summary>
        bool RemoveFromVariableSet<T>(string key, T item);

        /// <summary>
        /// 检查集合是否包含元素
        /// </summary>
        bool VariableSetContains<T>(string key, T item);

        // ========== 数值操作辅助方法 ==========

        /// <summary>
        /// 增加数值变量的值
        /// </summary>
        /// <param name="key">变量键名</param>
        /// <param name="increment">增加量</param>
        /// <returns>增加后的值</returns>
        int IncrementInt(string key, int increment = 1);

        /// <summary>
        /// 减少数值变量的值
        /// </summary>
        /// <param name="key">变量键名</param>
        /// <param name="decrement">减少量</param>
        /// <returns>减少后的值</returns>
        int DecrementInt(string key, int decrement = 1);

        /// <summary>
        /// 增加浮点数变量的值
        /// </summary>
        float IncrementFloat(string key, float increment = 1.0f);

        // ========== 批量操作 ==========

        /// <summary>
        /// 批量设置布尔标志
        /// </summary>
        void SetFlags(IEnumerable<KeyValuePair<string, bool>> flags);

        /// <summary>
        /// 批量设置变量
        /// </summary>
        void SetVariables(IEnumerable<KeyValuePair<string, object>> variables);

        // ========== 条件检查 ==========

        /// <summary>
        /// 检查数值条件
        /// </summary>
        bool CheckCondition(string variableKey, object compareValue);

        /// <summary>
        /// 检查多个标志是否都为真
        /// </summary>
        bool CheckAllFlags(params string[] keys);

        /// <summary>
        /// 检查多个标志中是否有任意一个为真
        /// </summary>
        bool CheckAnyFlag(params string[] keys);

        // ========== 持久化 ==========

        /// <summary>
        /// 保存游戏状态
        /// </summary>
        void SaveState(string saveSlot = "default");

        /// <summary>
        /// 加载游戏状态
        /// </summary>
        void LoadState(string saveSlot = "default");

        /// <summary>
        /// 重置游戏状态
        /// </summary>
        void ResetState(bool keepSystemFlags = false);

        // ========== 事件 ==========

        /// <summary>
        /// 当标志值变化时触发
        /// </summary>
        event Action<string, bool> OnFlagChanged;

        /// <summary>
        /// 当变量值变化时触发
        /// </summary>
        event Action<string, object> OnVariableChanged;
    }
}
