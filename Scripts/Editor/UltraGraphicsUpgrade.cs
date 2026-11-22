#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using PixelGunner.Player;
using PixelGunner.Core;
using PixelGunner.Enemies;
using PixelGunner.Items;
using PixelGunner.CameraSystem;
using PixelGunner.Managers;
using PixelGunner.UI;

namespace PixelGunner.EditorTools
{
    public static class UltraGraphicsUpgrade
    {
        [MenuItem("PixelGunner/★ NANG CAP DO HOA DEP NHAT (ULTRA PIXEL ART) ★")]
        public static void BuildUltraGraphicsScene()
        {
            if (EditorApplication.isPlaying) EditorApplication.isPlaying = false;

            string dir = "Assets/Sprites";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            // 1. Tạo trọn bộ Texture Ultra Pixel Art đỉnh cao
            string pSky = Path.Combine(dir, "bg_ultra_sky.png");
            string pMtn = Path.Combine(dir, "bg_ultra_mountains.png");
            string pSea = Path.Combine(dir, "bg_ultra_sea.png");
            string pCowboy = Path.Combine(dir, "spr_cowboy_master.png");
            string pBat = Path.Combine(dir, "spr_bat_master.png");
            string pSlime = Path.Combine(dir, "spr_slime_master.png");
            string pDemon = Path.Combine(dir, "spr_demon_master.png");
            string pGround = Path.Combine(dir, "spr_ground_master.png");
            string pPlat = Path.Combine(dir, "spr_plat_master.png");
            string pBush = Path.Combine(dir, "spr_bush_flowers.png");
            string pCoin = Path.Combine(dir, "spr_coin_gold.png");
            string pHeart = Path.Combine(dir, "spr_heart_ruby.png");
            string pPortal = Path.Combine(dir, "spr_portal_vortex.png");
            string pBlast = Path.Combine(dir, "spr_muzzle_blast.png");
            string pBullet = Path.Combine(dir, "spr_bullet_fire.png");

            File.WriteAllBytes(pSky, CreateUltraSky().EncodeToPNG());
            File.WriteAllBytes(pMtn, CreateUltraMountains().EncodeToPNG());
            File.WriteAllBytes(pSea, CreateUltraSea().EncodeToPNG());
            File.WriteAllBytes(pCowboy, CreateMasterCowboy().EncodeToPNG());
            File.WriteAllBytes(pBat, CreateMasterBat().EncodeToPNG());
            File.WriteAllBytes(pSlime, CreateMasterSlime().EncodeToPNG());
            File.WriteAllBytes(pDemon, CreateMasterDemon().EncodeToPNG());
            File.WriteAllBytes(pGround, CreateMasterGround().EncodeToPNG());
            File.WriteAllBytes(pPlat, CreateMasterPlatform().EncodeToPNG());
            File.WriteAllBytes(pBush, CreateBushWithFlowers().EncodeToPNG());
            File.WriteAllBytes(pCoin, CreateGoldCoin().EncodeToPNG());
            File.WriteAllBytes(pHeart, CreateRubyHeart().EncodeToPNG());
            File.WriteAllBytes(pPortal, CreateCosmicPortal().EncodeToPNG());
            File.WriteAllBytes(pBlast, CreateMuzzleBlast().EncodeToPNG());
            File.WriteAllBytes(pBullet, CreateFireBullet().EncodeToPNG());

            AssetDatabase.Refresh();

            // Cấu hình import Sprite chuẩn Pixel Art (Point filter, không mờ nhòe)
            SetupSprite(pSky, 32);
            SetupSprite(pMtn, 32);
            SetupSprite(pSea, 32);
            SetupSprite(pCowboy, 24);
            SetupSprite(pBat, 24);
            SetupSprite(pSlime, 24);
            SetupSprite(pDemon, 24);
            SetupSprite(pGround, 16);
            SetupSprite(pPlat, 16);
            SetupSprite(pBush, 20);
            SetupSprite(pCoin, 20);
            SetupSprite(pHeart, 20);
            SetupSprite(pPortal, 20);
            SetupSprite(pBlast, 20);
            SetupSprite(pBullet, 24);

            AssetDatabase.Refresh();

            // Load Sprites
            Sprite sprSky = AssetDatabase.LoadAssetAtPath<Sprite>(pSky);
            Sprite sprMtn = AssetDatabase.LoadAssetAtPath<Sprite>(pMtn);
            Sprite sprSea = AssetDatabase.LoadAssetAtPath<Sprite>(pSea);
            Sprite sprCowboy = AssetDatabase.LoadAssetAtPath<Sprite>(pCowboy);
            Sprite sprBat = AssetDatabase.LoadAssetAtPath<Sprite>(pBat);
            Sprite sprSlime = AssetDatabase.LoadAssetAtPath<Sprite>(pSlime);
            Sprite sprDemon = AssetDatabase.LoadAssetAtPath<Sprite>(pDemon);
            Sprite sprGround = AssetDatabase.LoadAssetAtPath<Sprite>(pGround);
            Sprite sprPlat = AssetDatabase.LoadAssetAtPath<Sprite>(pPlat);
            Sprite sprBush = AssetDatabase.LoadAssetAtPath<Sprite>(pBush);
            Sprite sprCoin = AssetDatabase.LoadAssetAtPath<Sprite>(pCoin);
            Sprite sprHeart = AssetDatabase.LoadAssetAtPath<Sprite>(pHeart);
            Sprite sprPortal = AssetDatabase.LoadAssetAtPath<Sprite>(pPortal);
            Sprite sprBlast = AssetDatabase.LoadAssetAtPath<Sprite>(pBlast);
            Sprite sprBullet = AssetDatabase.LoadAssetAtPath<Sprite>(pBullet);

            // 2. Tạo Scene Mới Tuyệt Đẹp
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Prefabs
            GameObject bulletPrefab = BuildBulletPrefab(sprBullet);
            GameObject blastPrefab = BuildBlastPrefab(sprBlast);

            // Camera
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            Camera cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.48f, 0.82f, 0.98f); // Trời xanh sáng
            cam.orthographic = true;
            cam.orthographicSize = 4.8f;
            camObj.transform.position = new Vector3(0, 1.8f, -10f);
            camObj.AddComponent<AudioListener>();

            // 3. Phông Nền Parallax Đa Tầng (Sky -> Mountains -> Sea)
            GameObject bgParent = new GameObject("--- PARALLAX BACKGROUND ---");

            // Tầng 1: Bầu trời & Mây xa (Layer -200)
            for (int i = -2; i <= 2; i++)
            {
                GameObject sky = new GameObject("Bg_Sky_" + i);
                sky.transform.SetParent(bgParent.transform);
                sky.transform.position = new Vector3(i * 18f, 5.5f, 15f);
                sky.transform.localScale = new Vector3(18f, 9f, 1f);
                var sr = sky.AddComponent<SpriteRenderer>();
                sr.sprite = sprSky;
                sr.sortingOrder = -200;
            }

            // Tầng 2: Dãy núi xa xăm & Đảo ngọc (Layer -150)
            for (int i = -2; i <= 2; i++)
            {
                GameObject mtn = new GameObject("Bg_Mountains_" + i);
                mtn.transform.SetParent(bgParent.transform);
                mtn.transform.position = new Vector3(i * 18f, 2.8f, 12f);
                mtn.transform.localScale = new Vector3(18f, 5.5f, 1f);
                var sr = mtn.AddComponent<SpriteRenderer>();
                sr.sprite = sprMtn;
                sr.sortingOrder = -150;
            }

            // Tầng 3: Biển xanh sóng vỗ lấp lánh (Layer -100)
            for (int i = -2; i <= 2; i++)
            {
                GameObject sea = new GameObject("Bg_Sea_" + i);
                sea.transform.SetParent(bgParent.transform);
                sea.transform.position = new Vector3(i * 18f, 0.2f, 10f);
                sea.transform.localScale = new Vector3(18f, 4.5f, 1f);
                var sr = sea.AddComponent<SpriteRenderer>();
                sr.sprite = sprSea;
                sr.sortingOrder = -100;
            }

            // 4. Môi trường Mặt Đất & Bụi Hoa Rực Rỡ
            GameObject env = new GameObject("--- ENVIRONMENT ---");

            // Mặt đất chính dài 60m
            GameObject ground = new GameObject("Main_Ground");
            ground.transform.SetParent(env.transform);
            ground.transform.position = new Vector3(0, -1f, 0);
            ground.tag = "Ground";
            var gSr = ground.AddComponent<SpriteRenderer>();
            gSr.sprite = sprGround;
            gSr.drawMode = SpriteDrawMode.Tiled;
            gSr.size = new Vector2(60f, 2.2f);
            gSr.sortingOrder = 0;
            var gCol = ground.AddComponent<BoxCollider2D>();
            gCol.size = new Vector2(60f, 2.2f);

            // Rải các bụi cây & hoa dại rực rỡ dọc đường
            for (float x = -22f; x <= 22f; x += 2.8f)
            {
                GameObject bush = new GameObject("Bush_Flower");
                bush.transform.SetParent(env.transform);
                bush.transform.position = new Vector3(x + Random.Range(-0.5f, 0.5f), 0.38f, 0);
                bush.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
                var bSr = bush.AddComponent<SpriteRenderer>();
                bSr.sprite = sprBush;
                bSr.sortingOrder = 3;
            }

            // Tường chắn vô hình 2 bên mép
            BuildWall(env.transform, "Wall_Left", new Vector3(-28f, 5f, 0));
            BuildWall(env.transform, "Wall_Right", new Vector3(28f, 5f, 0));

            // Các bục gỗ mỹ thuật
            BuildPlat(env.transform, "Platform_1", new Vector3(3.5f, 1.5f, 0), 4.5f, sprPlat);
            BuildPlat(env.transform, "Platform_2", new Vector3(9f, 3.2f, 0), 5f, sprPlat);
            BuildPlat(env.transform, "Platform_3", new Vector3(-6f, 2f, 0), 4.5f, sprPlat);

            // 5. Nhân vật Cao Bồi Siêu Đẹp (Player)
            GameObject player = new GameObject("Player");
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1.2f, 0);
            player.transform.localScale = new Vector3(2.1f, 2.1f, 1f);

            var pSr = player.AddComponent<SpriteRenderer>();
            pSr.sprite = sprCowboy;
            pSr.sortingOrder = 15;

            var pRb = player.AddComponent<Rigidbody2D>();
            pRb.gravityScale = 3f;
            pRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            pRb.freezeRotation = true;

            var pCol = player.AddComponent<BoxCollider2D>();
            pCol.size = new Vector2(0.75f, 1.35f);
            pCol.offset = new Vector2(0, 0.68f);

            player.AddComponent<Health>();

            // GroundCheck
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(player.transform);
            gc.transform.localPosition = new Vector3(0, 0.05f, 0);

            var pCtrl = player.AddComponent<PlayerController>();
            SetField(pCtrl, "groundCheckPoint", gc.transform);
            SetField(pCtrl, "groundLayer", (LayerMask)(~0));

            // Súng nòng ngắn & Điểm bắn
            GameObject gun = new GameObject("Gun");
            gun.transform.SetParent(player.transform);
            gun.transform.localPosition = new Vector3(0.4f, 0.72f, 0);

            GameObject muzzle = new GameObject("MuzzlePoint");
            muzzle.transform.SetParent(gun.transform);
            muzzle.transform.localPosition = new Vector3(0.6f, 0.02f, 0);

            var gunCtrl = gun.AddComponent<GunController>();
            SetField(gunCtrl, "bulletPrefab", bulletPrefab);
            SetField(gunCtrl, "muzzlePoint", muzzle.transform);
            SetField(gunCtrl, "muzzleFlashEffect", blastPrefab);

            // Camera follow Player
            var follow = camObj.AddComponent<CameraFollow2D>();
            SetField(follow, "target", player.transform);

            // 6. Quái Vật Dơi Ma Quái (Gargoyle Bat)
            BuildBat(new Vector3(7f, 2.6f, 0), sprBat);

            // 7. Slime Xanh Bay Lơ Lửng (Water Slime)
            BuildSlime(new Vector3(1.8f, 3.8f, 0), sprSlime);

            // 8. Quái Vật Đất (Armored Demon)
            BuildDemon(new Vector3(8.5f, 0.6f, 0), sprDemon);

            // 9. Vật phẩm Vàng & Tim Ruby
            BuildCoin(new Vector3(3.5f, 2.3f, 0), sprCoin);
            BuildHeart(new Vector3(9f, 4f, 0), sprHeart);
            BuildCoin(new Vector3(-6f, 2.8f, 0), sprCoin);

            // 10. Cánh Cổng Vũ Trụ Về Đích (Cosmic Portal)
            BuildPortal(new Vector3(18f, 0.6f, 0), sprPortal);

            // 11. GameManager & Canvas UI
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();

            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvasObj.AddComponent<UIManager>();

            // Lưu Scene và Mở Scene
            string scenePath = "Assets/Scenes/MainGame.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.OpenScene(scenePath);

            EditorUtility.DisplayDialog(
                "ĐÃ KHÔI PHỤC ĐỒ HỌA ĐỈNH CAO (ULTRA PIXEL ART)!",
                "Toàn bộ đồ họa đẹp nhất đã sẵn sàng:\n- Bầu trời Parallax 3 tầng: Mây xa -> Dãy núi ngọc -> Biển sóng biếc\n- Chàng Cao Bồi sắc nét cầm súng săn uy lực\n- Dơi ma quái tóc xanh, Slime xanh bay & Quái vật đất\n- Bụi hoa rực rỡ, tiền vàng, tim ruby & Cổng vũ trụ\n- Bảng CHIẾN THẮNG hiện lên khi hạ hết quái kèm nút CHƠI LẠI mượt mà 100%!\n\nNhấn PLAY (▶) để trải nghiệm!",
                "Chơi Ngay!"
            );
        }

        private static void BuildBat(Vector3 pos, Sprite spr)
        {
            GameObject bat = new GameObject("Enemy_Bat_Ultra");
            bat.tag = "Enemy";
            bat.transform.position = pos;
            bat.transform.localScale = new Vector3(2.1f, 2.1f, 1f);

            var sr = bat.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 16;

            var col = bat.AddComponent<CircleCollider2D>();
            col.radius = 0.55f;

            bat.AddComponent<Health>();
            bat.AddComponent<EnemyFlying>();
        }

        private static void BuildSlime(Vector3 pos, Sprite spr)
        {
            GameObject slime = new GameObject("Enemy_Slime_Ultra");
            slime.tag = "Enemy";
            slime.transform.position = pos;
            slime.transform.localScale = new Vector3(1.8f, 1.8f, 1f);

            var sr = slime.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 14;

            var col = slime.AddComponent<CircleCollider2D>();
            col.radius = 0.45f;

            slime.AddComponent<Health>();
            slime.AddComponent<EnemyFlying>();
        }

        private static void BuildDemon(Vector3 pos, Sprite spr)
        {
            GameObject demon = new GameObject("Enemy_Demon_Ultra");
            demon.tag = "Enemy";
            demon.transform.position = pos;
            demon.transform.localScale = new Vector3(2f, 2f, 1f);

            var sr = demon.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 12;

            var rb = demon.AddComponent<Rigidbody2D>();
            rb.gravityScale = 3f;
            rb.freezeRotation = true;

            var col = demon.AddComponent<BoxCollider2D>();
            col.size = new Vector2(0.9f, 0.95f);
            col.offset = new Vector2(0, 0.48f);

            demon.AddComponent<Health>();

            GameObject pA = new GameObject("PointA");
            pA.transform.position = new Vector3(pos.x - 3.5f, pos.y, 0);
            GameObject pB = new GameObject("PointB");
            pB.transform.position = new Vector3(pos.x + 5f, pos.y, 0);

            var patrol = demon.AddComponent<EnemyPatrol>();
            SetField(patrol, "pointA", pA.transform);
            SetField(patrol, "pointB", pB.transform);
        }

        private static void SetupSprite(string path, int ppu)
        {
            TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;
            if (imp != null)
            {
                imp.textureType = TextureImporterType.Sprite;
                imp.spriteImportMode = SpriteImportMode.Single;
                imp.spritePixelsPerUnit = ppu;
                imp.filterMode = FilterMode.Point;
                imp.textureCompression = TextureImporterCompression.Uncompressed;
                imp.SaveAndReimport();
            }
        }

        private static GameObject BuildBulletPrefab(Sprite spr)
        {
            GameObject b = new GameObject("Bullet_Ultra_Prefab");
            var sr = b.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 25;
            b.transform.localScale = new Vector3(1.6f, 1.6f, 1f);

            var col = b.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.25f;

            b.AddComponent<Bullet>();

            string path = "Assets/Prefabs/Bullet_Ultra.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(b, path);
            Object.DestroyImmediate(b);
            return prefab;
        }

        private static GameObject BuildBlastPrefab(Sprite spr)
        {
            GameObject f = new GameObject("Blast_Effect");
            var sr = f.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 30;
            f.transform.localScale = new Vector3(2f, 2f, 1f);

            string path = "Assets/Prefabs/Blast_Effect.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(f, path);
            Object.DestroyImmediate(f);
            return prefab;
        }

        private static void BuildWall(Transform parent, string name, Vector3 pos)
        {
            GameObject wall = new GameObject(name);
            wall.transform.SetParent(parent);
            wall.transform.position = pos;
            var col = wall.AddComponent<BoxCollider2D>();
            col.size = new Vector2(2f, 25f);
        }

        private static void BuildPlat(Transform parent, string name, Vector3 pos, float width, Sprite spr)
        {
            GameObject p = new GameObject(name);
            p.transform.SetParent(parent);
            p.transform.position = pos;
            p.tag = "Ground";

            var sr = p.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.drawMode = SpriteDrawMode.Tiled;
            sr.size = new Vector2(width, 0.65f);
            sr.sortingOrder = 2;

            var col = p.AddComponent<BoxCollider2D>();
            col.size = new Vector2(width, 0.65f);
        }

        private static void BuildCoin(Vector3 pos, Sprite spr)
        {
            GameObject coin = new GameObject("Item_Coin_Gold");
            coin.transform.position = pos;
            coin.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
            var sr = coin.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 8;
            var col = coin.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            var item = coin.AddComponent<ItemPickup>();
            SetField(item, "itemType", ItemType.Coin);
        }

        private static void BuildHeart(Vector3 pos, Sprite spr)
        {
            GameObject heart = new GameObject("Item_Heart_Ruby");
            heart.transform.position = pos;
            heart.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
            var sr = heart.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 8;
            var col = heart.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            var item = heart.AddComponent<ItemPickup>();
            SetField(item, "itemType", ItemType.HealthPack);
        }

        private static void BuildPortal(Vector3 pos, Sprite spr)
        {
            GameObject portal = new GameObject("Level_Portal_Cosmic");
            portal.transform.position = pos;
            portal.transform.localScale = new Vector3(2.2f, 2.2f, 1f);
            var sr = portal.AddComponent<SpriteRenderer>();
            sr.sprite = spr;
            sr.sortingOrder = 6;
            var col = portal.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            portal.AddComponent<LevelPortal>();
        }

        private static void SetField(object target, string name, object val)
        {
            var f = target.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (f != null) f.SetValue(target, val);
        }

        #region VẼ ĐỒ HỌA PIXEL ART SIÊU ĐẸP
        private static Texture2D CreateUltraSky()
        {
            // Bầu trời chuyển sắc + Mây xốp bồng bềnh
            Texture2D t = new Texture2D(128, 64);
            Color skyTop = new Color(0.42f, 0.72f, 0.98f);
            Color skyBot = new Color(0.78f, 0.92f, 1f);
            Color cloudMain = new Color(1f, 1f, 1f, 0.92f);
            Color cloudSh = new Color(0.82f, 0.88f, 0.96f, 0.85f);

            for (int y = 0; y < 64; y++)
            {
                Color col = Color.Lerp(skyBot, skyTop, (float)y / 64f);
                for (int x = 0; x < 128; x++) t.SetPixel(x, y, col);
            }

            DrawEllipse(t, 30, 40, 22, 10, cloudMain, cloudSh);
            DrawEllipse(t, 90, 48, 28, 12, cloudMain, cloudSh);
            DrawEllipse(t, 115, 25, 18, 8, cloudMain, cloudSh);

            t.Apply(); return t;
        }

        private static Texture2D CreateUltraMountains()
        {
            // Dãy núi tím mộng mơ xa xăm
            Texture2D t = new Texture2D(128, 48); Clear(t);
            Color mtn1 = new Color(0.38f, 0.55f, 0.75f, 0.85f);
            Color mtn2 = new Color(0.28f, 0.42f, 0.62f, 0.9f);
            Color peak = new Color(0.85f, 0.92f, 0.98f, 0.9f);

            for (int x = 0; x < 128; x++)
            {
                int h1 = (int)(Mathf.Sin(x * 0.08f) * 12f + Mathf.Cos(x * 0.03f) * 8f + 22f);
                for (int y = 0; y <= h1 && y < 48; y++)
                {
                    if (y >= h1 - 3 && h1 > 28) t.SetPixel(x, y, peak); // Tuyết trên đỉnh núi
                    else t.SetPixel(x, y, (x % 7 == 0) ? mtn2 : mtn1);
                }
            }
            t.Apply(); return t;
        }

        private static Texture2D CreateUltraSea()
        {
            // Biển xanh ngọc sóng vỗ dập dềnh
            Texture2D t = new Texture2D(128, 32);
            Color seaFar = new Color(0.18f, 0.68f, 0.82f);
            Color seaNear = new Color(0.08f, 0.52f, 0.68f);
            Color foam = new Color(1f, 1f, 1f, 0.9f);

            for (int y = 0; y < 32; y++)
            {
                Color col = Color.Lerp(seaNear, seaFar, (float)y / 32f);
                for (int x = 0; x < 128; x++)
                {
                    bool wave = ((x + y * 4) % 15 == 0 && y > 8);
                    t.SetPixel(x, y, wave ? foam : col);
                }
            }
            t.Apply(); return t;
        }

        private static Texture2D CreateMasterCowboy()
        {
            // Chàng Cao Bồi 32x32 Đỉnh Cao
            Texture2D t = new Texture2D(32, 32); Clear(t);
            Color hat = new Color(0.55f, 0.32f, 0.15f);
            Color hatD = new Color(0.42f, 0.24f, 0.1f);
            Color star = new Color(1f, 0.85f, 0.2f);
            Color skin = new Color(0.98f, 0.82f, 0.68f);
            Color eye = new Color(0.2f, 0.55f, 0.85f);
            Color shirt = Color.white;
            Color vest = new Color(0.45f, 0.22f, 0.1f);
            Color belt = new Color(0.3f, 0.18f, 0.08f);
            Color pants = new Color(0.2f, 0.25f, 0.38f);
            Color boots = new Color(0.35f, 0.2f, 0.1f);
            Color gunMetal = new Color(0.25f, 0.25f, 0.3f);
            Color gunWood = new Color(0.6f, 0.35f, 0.18f);

            // Mũ cao bồi có vành uốn cong (Y: 20 -> 29)
            Fill(t, 3, 21, 26, 2, hat);
            Fill(t, 1, 22, 30, 2, hat);
            Fill(t, 7, 24, 18, 5, hat);
            Fill(t, 9, 29, 14, 1, hatD);
            Fill(t, 7, 24, 18, 1, hatD); // Dải da quanh mũ
            t.SetPixel(15, 26, star); t.SetPixel(16, 26, star); t.SetPixel(15, 27, star); // Ngôi sao cảnh sát

            // Khuôn mặt (Y: 13 -> 20)
            Fill(t, 10, 14, 12, 7, skin);
            Fill(t, 8, 16, 3, 4, new Color(0.75f, 0.55f, 0.25f)); // Tóc vàng
            // Mắt xanh sống động
            Fill(t, 17, 16, 3, 2, Color.white); t.SetPixel(18, 16, eye); t.SetPixel(18, 17, Color.black);
            Fill(t, 12, 16, 3, 2, Color.white); t.SetPixel(13, 16, eye); t.SetPixel(13, 17, Color.black);

            // Áo trắng & Khăn quàng đỏ rực
            Fill(t, 10, 7, 12, 6, shirt);
            Fill(t, 9, 8, 3, 5, vest); Fill(t, 20, 8, 3, 5, vest);
            Fill(t, 13, 12, 6, 2, new Color(0.9f, 0.2f, 0.2f)); // Khăn bandana đỏ

            // Khẩu súng săn Shotgun trên tay
            Fill(t, 18, 10, 12, 3, gunMetal);
            Fill(t, 16, 9, 4, 3, gunWood); // Báng gỗ

            // Thắt lưng đạn & bao súng
            Fill(t, 9, 6, 14, 2, belt);
            t.SetPixel(15, 6, star); t.SetPixel(16, 6, star);
            t.SetPixel(11, 7, Color.yellow); t.SetPixel(13, 7, Color.yellow); // Vỏ đạn găm thắt lưng

            // Quần jean xanh
            Fill(t, 10, 2, 5, 4, pants); Fill(t, 17, 2, 5, 4, pants);

            // Bốt da cao bồi có đinh sắt
            Fill(t, 9, 0, 6, 2, boots); Fill(t, 17, 0, 6, 2, boots);
            t.SetPixel(8, 0, star); // Đinh thúc ngựa vàng

            t.Apply(); return t;
        }

        private static Texture2D CreateMasterBat()
        {
            // Dơi Ma Quái 32x32 Chuẩn Video (Cánh đen tím, mào xanh lá, mũi hồng, nanh trắng)
            Texture2D t = new Texture2D(32, 32); Clear(t);
            Color body = new Color(0.2f, 0.15f, 0.24f);
            Color wing = new Color(0.3f, 0.2f, 0.35f);
            Color mohawk = new Color(0.35f, 0.9f, 0.2f); // Mào xanh lá
            Color nose = new Color(0.95f, 0.5f, 0.65f); // Mũi hồng
            Color gem = new Color(0.7f, 0.2f, 0.95f); // Ngọc tím trên đầu

            // Viên ngọc ma thuật lơ lửng trên đầu
            FillCircle(t, 16, 29, 3, gem);
            t.SetPixel(16, 30, Color.white);

            // Thân dơi mập mạp
            FillCircle(t, 16, 14, 8, body);

            // Mào tóc nhọn xanh dựng ngược
            Fill(t, 11, 22, 4, 5, mohawk);
            Fill(t, 14, 22, 4, 6, mohawk);
            Fill(t, 17, 22, 3, 4, mohawk);

            // Mắt to hung tợn
            Fill(t, 9, 14, 5, 4, Color.white); t.SetPixel(11, 15, Color.black);
            Fill(t, 17, 14, 5, 4, Color.white); t.SetPixel(18, 15, Color.black);

            // Mũi lợn hồng
            Fill(t, 13, 11, 6, 3, nose);
            t.SetPixel(14, 12, Color.black); t.SetPixel(17, 12, Color.black);

            // Răng nanh trắng nhọn
            t.SetPixel(12, 9, Color.white); t.SetPixel(12, 8, Color.white);
            t.SetPixel(19, 9, Color.white); t.SetPixel(19, 8, Color.white);

            // Cánh dơi lớn xòe rộng
            Fill(t, 1, 13, 9, 11, wing); t.SetPixel(0, 23, wing);
            Fill(t, 22, 13, 9, 11, wing); t.SetPixel(31, 23, wing);

            t.Apply(); return t;
        }

        private static Texture2D CreateMasterSlime()
        {
            // Slime Xanh Tai Mèo Lơ Lửng 24x24
            Texture2D t = new Texture2D(24, 24); Clear(t);
            Color slime = new Color(0.4f, 0.78f, 0.98f, 0.95f);
            Color shine = new Color(0.75f, 0.95f, 1f, 0.95f);

            FillCircle(t, 12, 10, 8, slime);
            FillCircle(t, 10, 13, 4, shine); // Đốm sáng

            // Tai nhọn dễ thương
            t.SetPixel(7, 17, slime); t.SetPixel(6, 18, slime); t.SetPixel(5, 19, slime);
            t.SetPixel(17, 17, slime); t.SetPixel(18, 18, slime); t.SetPixel(19, 19, slime);

            // Mắt tròn xoe
            t.SetPixel(9, 10, Color.black); t.SetPixel(9, 11, Color.black);
            t.SetPixel(15, 10, Color.black); t.SetPixel(15, 11, Color.black);

            // Má hồng
            t.SetPixel(7, 8, new Color(0.95f, 0.55f, 0.7f));
            t.SetPixel(17, 8, new Color(0.95f, 0.55f, 0.7f));

            t.Apply(); return t;
        }

        private static Texture2D CreateMasterDemon()
        {
            // Quái Vật Đất Thiết Giáp 28x28
            Texture2D t = new Texture2D(28, 28); Clear(t);
            Color red = new Color(0.85f, 0.2f, 0.2f);
            Color dark = new Color(0.55f, 0.1f, 0.1f);
            Color horn = new Color(0.95f, 0.85f, 0.4f);
            Color armor = new Color(0.35f, 0.35f, 0.4f);

            Fill(t, 6, 2, 16, 18, red);
            Fill(t, 4, 6, 20, 11, red);

            // Giáp vai sắt
            Fill(t, 3, 10, 4, 6, armor); Fill(t, 21, 10, 4, 6, armor);

            // Sừng quỷ vàng cong vút
            t.SetPixel(6, 21, horn); t.SetPixel(5, 22, horn); t.SetPixel(4, 23, horn);
            t.SetPixel(21, 21, horn); t.SetPixel(22, 22, horn); t.SetPixel(23, 23, horn);

            // Mắt vàng rực lửa
            Fill(t, 8, 12, 4, 4, Color.yellow); t.SetPixel(10, 13, Color.black);
            Fill(t, 16, 12, 4, 4, Color.yellow); t.SetPixel(17, 13, Color.black);

            // Răng nanh sắc lẹm
            Fill(t, 9, 6, 10, 3, dark);
            t.SetPixel(10, 8, Color.white); t.SetPixel(17, 8, Color.white);

            t.Apply(); return t;
        }

        private static Texture2D CreateMasterGround()
        {
            // Mặt đất cỏ dày với rễ cây & sỏi đá 32x32
            Texture2D t = new Texture2D(32, 32);
            Color gTop = new Color(0.38f, 0.88f, 0.28f);
            Color gMid = new Color(0.25f, 0.68f, 0.18f);
            Color dirt1 = new Color(0.55f, 0.35f, 0.2f);
            Color dirt2 = new Color(0.42f, 0.26f, 0.14f);
            Color stone = new Color(0.6f, 0.6f, 0.65f);

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    if (y >= 26) t.SetPixel(x, y, gTop);
                    else if (y >= 21)
                    {
                        // Rễ cỏ cắm xuống
                        bool overhang = (x % 3 == 0 && y == 21);
                        t.SetPixel(x, y, overhang ? gTop : gMid);
                    }
                    else
                    {
                        // Sỏi đá ngẫu nhiên trong lòng đất
                        bool isStone = (x % 8 == 2 && y % 7 == 3);
                        t.SetPixel(x, y, isStone ? stone : (((x + y) % 4 == 0) ? dirt2 : dirt1));
                    }
                }
            }
            t.Apply(); return t;
        }

        private static Texture2D CreateMasterPlatform()
        {
            Texture2D t = new Texture2D(32, 16);
            Color w1 = new Color(0.68f, 0.45f, 0.24f);
            Color w2 = new Color(0.48f, 0.3f, 0.15f);
            Color ivy = new Color(0.35f, 0.82f, 0.25f); // Dây leo xanh

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    t.SetPixel(x, y, (y == 0 || y == 15 || y % 5 == 0) ? w2 : w1);
                }
            }
            // Dây leo cỏ phủ trên bục gỗ
            for (int x = 0; x < 32; x += 4)
            {
                t.SetPixel(x, 15, ivy); t.SetPixel(x + 1, 14, ivy);
            }
            t.Apply(); return t;
        }

        private static Texture2D CreateBushWithFlowers()
        {
            // Bụi cỏ xanh điểm xuyết hoa đỏ hoa vàng 32x20
            Texture2D t = new Texture2D(32, 20); Clear(t);
            Color g1 = new Color(0.32f, 0.85f, 0.25f);
            Color g2 = new Color(0.2f, 0.62f, 0.15f);
            Color flowerRed = new Color(1f, 0.3f, 0.35f);
            Color flowerYellow = new Color(1f, 0.9f, 0.2f);

            // Bụi cỏ nhọn
            for (int x = 2; x < 30; x += 2)
            {
                int h = ((x * 5) % 10) + 8;
                Fill(t, x, 0, 2, h, g1);
                t.SetPixel(x, h, g2);
            }

            // Điểm những bông hoa dại nhỏ
            Fill(t, 6, 12, 3, 3, flowerRed); t.SetPixel(7, 13, Color.white);
            Fill(t, 18, 14, 3, 3, flowerYellow); t.SetPixel(19, 15, Color.white);
            Fill(t, 25, 10, 3, 3, flowerRed);

            t.Apply(); return t;
        }

        private static Texture2D CreateGoldCoin()
        {
            Texture2D t = new Texture2D(20, 20); Clear(t);
            Color gold = new Color(1f, 0.85f, 0.15f);
            Color dark = new Color(0.85f, 0.65f, 0.05f);

            FillCircle(t, 10, 10, 8, gold);
            FillCircle(t, 10, 10, 6, dark);
            FillCircle(t, 10, 10, 4, gold);
            t.SetPixel(8, 12, Color.white); t.SetPixel(9, 13, Color.white); // Đốm lấp lánh

            t.Apply(); return t;
        }

        private static Texture2D CreateRubyHeart()
        {
            Texture2D t = new Texture2D(20, 20); Clear(t);
            Color red = new Color(1f, 0.22f, 0.32f);
            Color dark = new Color(0.75f, 0.1f, 0.18f);

            // Hình trái tim ruby phát sáng
            FillCircle(t, 7, 12, 5, red);
            FillCircle(t, 13, 12, 5, red);
            for (int y = 0; y <= 10; y++)
            {
                int w = y + 1;
                Fill(t, 10 - w, y + 2, w * 2, 1, red);
            }
            t.SetPixel(6, 14, Color.white); // Đốm sáng

            t.Apply(); return t;
        }

        private static Texture2D CreateCosmicPortal()
        {
            Texture2D t = new Texture2D(24, 32); Clear(t);
            Color p1 = new Color(0.6f, 0.15f, 0.95f, 0.9f);
            Color p2 = new Color(0.15f, 0.85f, 1f, 0.9f);

            DrawEllipse(t, 12, 16, 9, 14, p1, p1);
            DrawEllipse(t, 12, 16, 6, 10, p2, p2);
            DrawEllipse(t, 12, 16, 3, 5, Color.white, Color.white);

            t.Apply(); return t;
        }

        private static Texture2D CreateMuzzleBlast()
        {
            Texture2D t = new Texture2D(24, 16); Clear(t);
            Color smoke = new Color(1f, 1f, 1f, 0.95f);
            Color yellow = new Color(1f, 0.85f, 0.2f);
            Color orange = new Color(1f, 0.5f, 0.1f);

            FillCircle(t, 6, 8, 5, smoke);
            FillCircle(t, 13, 8, 4, yellow);
            Fill(t, 16, 7, 7, 3, orange);

            t.Apply(); return t;
        }

        private static Texture2D CreateFireBullet()
        {
            Texture2D t = new Texture2D(12, 6); Clear(t);
            Fill(t, 0, 1, 9, 4, new Color(1f, 0.85f, 0.15f));
            Fill(t, 9, 2, 3, 2, new Color(1f, 0.45f, 0.05f));
            t.Apply(); return t;
        }

        private static void Clear(Texture2D t)
        {
            Color c = new Color(0, 0, 0, 0);
            for (int i = 0; i < t.width * t.height; i++) t.SetPixel(i % t.width, i / t.width, c);
        }

        private static void Fill(Texture2D t, int x, int y, int w, int h, Color c)
        {
            for (int j = y; j < y + h && j < t.height; j++)
                for (int i = x; i < x + w && i < t.width; i++)
                    if (i >= 0 && j >= 0) t.SetPixel(i, j, c);
        }

        private static void FillCircle(Texture2D t, int cx, int cy, int r, Color c)
        {
            for (int y = -r; y <= r; y++)
                for (int x = -r; x <= r; x++)
                    if (x * x + y * y <= r * r)
                    {
                        int px = cx + x; int py = cy + y;
                        if (px >= 0 && px < t.width && py >= 0 && py < t.height) t.SetPixel(px, py, c);
                    }
        }

        private static void DrawEllipse(Texture2D t, int cx, int cy, int rx, int ry, Color main, Color sh)
        {
            for (int y = -ry; y <= ry; y++)
            {
                for (int x = -rx; x <= rx; x++)
                {
                    if ((x * x) / (float)(rx * rx) + (y * y) / (float)(ry * ry) <= 1f)
                    {
                        int px = cx + x; int py = cy + y;
                        if (px >= 0 && px < t.width && py >= 0 && py < t.height)
                        {
                            t.SetPixel(px, py, (y < 0) ? sh : main);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
#endif
