using UnityEngine;
using Steamworks;
using UnityEngine.UI;

public class SteamTest : MonoBehaviour
{
    public Image UserProfileImage;
    public Text UserName;
    public Text UserSteamID;

    private void Start()
    {
        if (!SteamManager.Initialized) { return; } // 강민재 : 스팀 사용시 필수 요소

        string steamName = SteamFriends.GetPersonaName();
        CSteamID steamID = SteamUser.GetSteamID();
        Texture2D avatarTexture = GetSteamImageAsTexture2D(SteamFriends.GetLargeFriendAvatar(steamID));


        if (UserProfileImage != null && avatarTexture != null)
        {
            Sprite avatarSprite = Sprite.Create(avatarTexture, new Rect(0, 0, avatarTexture.width, avatarTexture.height), new Vector2(0.5f, 0.5f));
            UserProfileImage.sprite = avatarSprite;
        }
        else
        {
            Debug.Log("No, profile image rendering : None user profile image");
        }
        Debug.Log("Steam User : " + steamName);
        Debug.Log("Steam User ID : " + steamID);
        UserName.text = steamName;
        UserSteamID.text = steamID.ToString();
    }

    private static Texture2D GetSteamImageAsTexture2D(int iImage)
    {
        Texture2D ret = null;
        uint ImageWidth;
        uint ImageHeight;
        bool bIsValid = SteamUtils.GetImageSize(iImage, out ImageWidth, out ImageHeight);

        if (bIsValid)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];

            bIsValid = SteamUtils.GetImageRGBA(iImage, Image, (int)(ImageWidth * ImageHeight * 4));
            if (bIsValid)
            {
                ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
                ret.LoadRawTextureData(Image);
                ret.Apply();

                FlipTextureY(ret); // 강민재 : 이미지가 반전되어서 보여서 일단 추가함
            }
        }

        return ret;
    }

    private static void FlipTextureY(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        int height = texture.height;

        for (int y = 0; y < height / 2; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color temp = pixels[y * texture.width + x];
                pixels[y * texture.width + x] = pixels[(height - 1 - y) * texture.width + x];
                pixels[(height - 1 - y) * texture.width + x] = temp;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }
}
