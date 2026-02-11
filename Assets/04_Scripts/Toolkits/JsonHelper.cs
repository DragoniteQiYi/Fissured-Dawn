using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FissuredDawn.Toolkits
{
    public static class JsonHelper
    {
        /// <summary>
        /// 从Addressables异步加载并反序列化JSON文件
        /// </summary>
        /// <typeparam name="T">反序列化的目标类型</typeparam>
        /// <param name="assetPath">Addressables资源路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>反序列化的对象</returns>
        public static async UniTask<T> LoadAsync<T>(
            string assetPath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogError("[JsonHelper]: 所给JSON文件资源ID无效-" + nameof(assetPath));
            }

            try
            {
                // 加载TextAsset
                AsyncOperationHandle<TextAsset> handle =
                    Addressables.LoadAssetAsync<TextAsset>(assetPath);

                // 等待加载完成，支持取消
                await handle.ToUniTask(cancellationToken: cancellationToken);

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    TextAsset jsonContent = handle.Result;

                    // 反序列化JSON
                    T result = JsonConvert.DeserializeObject<T>(jsonContent.text);

                    // 释放资源引用（但不真正卸载）
                    Addressables.Release(handle);

                    return result;
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Failed to load JSON from Addressables: {handle.OperationException?.Message}",
                        handle.OperationException);
                }
            }
            catch (OperationCanceledException)
            {
                // 操作被取消，重新抛出
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonHelper]: Failed to load JSON from '{assetPath}': {ex.Message}");
                throw;
            }
        }
    }
}
