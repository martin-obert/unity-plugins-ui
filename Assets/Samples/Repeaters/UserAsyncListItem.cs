using System.Collections;
using Obert.UI.Runtime.Placeholders;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Samples.Repeaters
{
    public sealed class UserAsyncListItem : MonoBehaviour
    {
        [SerializeField] private Image avatar;
        [SerializeField] private SkeletonPlaceholder skeleton;

        public void Bind(UserAsync data)
        {
            var webRequest = UnityWebRequestTexture.GetTexture(data.avatarUrl);


            skeleton.AsyncAction(LoadTask(webRequest), () =>
            {
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(webRequest.error);
                    return;
                }

                var texture = DownloadHandlerTexture.GetContent(webRequest);

                avatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
            });
        }

        private static IEnumerator LoadTask(UnityWebRequest webRequest)
        {
            yield return new WaitForSeconds(Random.Range(3, 9));
            yield return webRequest.SendWebRequest();
        }
    }
}