using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class World : MonoBehaviour
{  
    // how far the walls are out
    public int xSize = 200;
    public int ySize = 200;
    public int seed = 694;

    public GameObject tree;
    public GameObject stump;
    
    public Score scores;

    public List<Sprite> trees = new List<Sprite>(1);
    public List<GameObject> rocks = new List<GameObject>(1);
    
    [SerializeField]
    private List<Vector4> treepos = new List<Vector4>();
    private List<Vector3> treeStumps = new List<Vector3>();
    [SerializeField]
    private List<Vector4> rockpos = new List<Vector4>();

    public GameObject rockParents;
    public GameObject treeParnets;
    private Vector2 forward = new Vector2(0, 1);
    private Vector2 backward = new Vector2(0, -1);
    private Vector2 left = new Vector2(-1, 0);
    private Vector2 right = new Vector2(1, 0);
    
    public bool isDoneLoading = false;

    // Start is called before the first frame update
    public void Init()
    {
        rockParents.name = "rockParent";
        treeParnets.name = "treeParent";
        StartCoroutine(createWorld());
    }

    IEnumerator createWorld( ) {
        for (int i = -xSize; i < xSize; i++){
            for(int j = -ySize; j < ySize; j++){
                double nv = getInterpolatedNoise(i*128, j*128)*120;
                if (nv > 10){
                    if (nv > 20){
                        int randomPOS = UnityEngine.Random.Range (0,(rocks.Count));
                        GameObject r = rocks[randomPOS];
                        r.transform.position = new Vector3(i,j,0);
                        RaycastHit2D objs = Physics2D.BoxCast(r.transform.position, r.GetComponent<BoxCollider2D>().size/2, 0, forward);
                        if (objs.collider == null || objs.collider.tag == "Trigger"){
                            GameObject go = Instantiate(r);
                            go.transform.parent = rockParents.transform;
                            SpriteRenderer re = go.GetComponent<SpriteRenderer>();
                            if (re != null)
                                re.enabled = false;
                            else{
                                SpriteRenderer rec = go.GetComponentInChildren<SpriteRenderer>();
                                rec.enabled = false;
                            }
                            //go.SetActive(false);
                            rockpos.Add(new Vector4(go.transform.position.x, go.transform.position.y, go.transform.position.z, randomPOS));
                        }
                    } else {
                        int randomPOS = UnityEngine.Random.Range (0,(trees.Count));
                        Sprite sp = trees[randomPOS];
                        tree.transform.position = new Vector3(i,j,0);
                        RaycastHit2D objs = Physics2D.BoxCast(tree.transform.position, tree.GetComponent<BoxCollider2D>().size/2, 0, forward);
                        if (objs.collider == null || objs.collider.tag == "Trigger"){
                            GameObject go = Instantiate(tree);
                            go.transform.parent = treeParnets.transform;
                            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                            sr.enabled = false;
                            sr.sprite = sp;
                            go.GetComponent<TreeController>().enabled = false;
                            //go.SetActive(false);
                            treepos.Add(new Vector4(go.transform.position.x, go.transform.position.y, go.transform.position.z, randomPOS));
                        }   
                    }
                }
                
            }
            yield return new WaitForEndOfFrame();
        }
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ChangeState(1);
    }

    public void TreeChopped(GameObject go){
        Vector3 posDD = go.transform.position;
        treeStumps.Add(posDD);
        StartCoroutine(RemoveTreeObject(posDD));
    }

    IEnumerator RemoveTreeObject(Vector3 posDD) {
        for (int i = 0; i < treepos.Count; i++){
            Vector4 treePosDD = treepos[i];
            if (treePosDD.x == posDD.x && treePosDD.y == posDD.y && treePosDD.z == posDD.z)
                treepos.RemoveAt(i);
        }
        yield return new WaitForEndOfFrame();
    }

    public void loadWorld(){
        StartCoroutine(GetPlayerObject());
    }

    IEnumerator GetPlayerObject() {
        GameObject go = null;
        GameObject fire = null;
        while (go == null || fire == null){
            go = GameObject.FindGameObjectWithTag("Player");
            fire = GameObject.FindGameObjectWithTag("Fire");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Got the player!");
        if (File.Exists(Application.dataPath + "/player.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "/player.dat", FileMode.Open);
            
            SaveDataStore data = bf.Deserialize(stream) as SaveDataStore;
            PlayerController con = go.GetComponent<PlayerController>();
            FireStates firest = fire.GetComponent<FireStates>();
            
            this.rockpos = data.getRockpos();
            this.treepos = data.getTreepos();
            this.treeStumps = data.getTreeStumppos();
            con.health = data.health;
            con.numLogs = data.numLogs;
            con.hasBackpack = data.hasBackpack;
            go.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            scores.name = data.name;
            scores.score = data.score;
            firest.logs = data.fireLogs;
            firest.fireStrength = data.fireStrength;
            for (int i = 0; i < rockpos.Count; i++){
                GameObject sp = rocks[(int)rockpos[i].w];
                sp.transform.parent = rockParents.transform;
                sp.transform.position = new Vector3(rockpos[i].x, rockpos[i].y, rockpos[i].z);
                SpriteRenderer sr = sp.GetComponent<SpriteRenderer>();
                if (sr == null)
                    sr = sp.GetComponentInChildren<SpriteRenderer>();
                sr.enabled = false;
                Instantiate(sp);
            }
            for (int i = 0; i < treepos.Count; i++){
                GameObject sp = tree;
                sp.transform.parent = treeParnets.transform;
                sp.transform.position = new Vector3(treepos[i].x, treepos[i].y, treepos[i].z);
                SpriteRenderer sr = sp.GetComponent<SpriteRenderer>();
                if (sr == null)
                    sr = sp.GetComponentInChildren<SpriteRenderer>();
                sr.sprite = trees[(int)treepos[i].w];
                sr.enabled = false;
                sp.GetComponent<TreeController>().enabled = false;
                Instantiate(sp);
            }
            for (int i = 0; i < treeStumps.Count; i++){
                GameObject sp = stump;
                sp.transform.parent = treeParnets.transform;
                sp.transform.position = treeStumps[i];
                SpriteRenderer sr = sp.GetComponent<SpriteRenderer>();
                if (sr == null)
                    sr = sp.GetComponentInChildren<SpriteRenderer>();
                sr.enabled = false;
                Instantiate(sp);
            }
            Debug.Log("Game Loaded!");
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ChangeState(1);
        } else {
            Debug.Log("Missing Player file!");
            this.Init();
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>().ChangeState(1);
        }
    }

    void OnApplicationQuit()
    {
        saveWorld();
    }

    public void saveWorld(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject fire = GameObject.FindGameObjectWithTag("Fire");
        if (player == null)
            return;
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.dataPath + "/player.dat", FileMode.Create);

        PlayerController con = player.GetComponent<PlayerController>();
        FireStates firest = fire.GetComponent<FireStates>();

        SaveDataStore data = new SaveDataStore(treepos, treeStumps, rockpos, con.health, con.numLogs, con.hasBackpack, player.transform.position, scores.name, scores.score, firest.fireStrength, firest.logs);

        bf.Serialize(stream, data);

        stream.Close();
        Debug.Log("Player data saved!");
    }

    public float getInterpolatedNoise(float x, float z) {
		int intX = (int) x;
        int intZ = (int) z;
        float fracX = x - intX;
        float fracZ = z - intZ;
         
        float v1 = getSmoothNoise(intX, intZ);
        float v2 = getSmoothNoise(intX + 1, intZ);
        float v3 = getSmoothNoise(intX, intZ + 1);
        float v4 = getSmoothNoise(intX + 1, intZ + 1);
        float i1 = interpolate(v1, v2, fracX);
        float i2 = interpolate(v3, v4, fracX);
        return interpolate(i1, i2, fracZ);
		//return (float) (Math.cos(x)+Math.sin(z));
	}
	
	private float interpolate(float a, float b, float blend) {
		float theta = blend * Mathf.PI;
        float f = (float)(1f - Mathf.Cos(theta)) * 0.5f;
        return a * (1f - f) + b * f;
	}
	
	private float getSmoothNoise(int x, int z) {
		float corners = (getNoise(x - 1, z - 1) + getNoise(x + 1, z - 1) + getNoise(x - 1, z + 1) + getNoise(x + 1, z + 1)) / 32f;
		float sides = (getNoise(x - 1, z) + getNoise(x + 1, z) + getNoise(x, z - 1) + getNoise(x, z + 1)) / 15f;
		float center = getNoise(x, z) / 8f;
		return corners + sides + center;
	}
	
	private float getNoise(int x, int z) {
		UnityEngine.Random.InitState((int) (x * 49632 + z * 325176 + seed));
		return (float)UnityEngine.Random.value * 2f - 1f;
	}

}

[Serializable]
public class SaveDataStore {

    public List<float[]> treepos = new List<float[]>();
    public List<float[]> rockpos = new List<float[]>();
    public List<float[]> treeStumps = new List<float[]>();
    public float health;
    public int numLogs;
    public bool hasBackpack;
    public float[] position;
    public string name;
    public float score;
    public float fireStrength;
    public int fireLogs;

    public SaveDataStore(List<Vector4> treepos, List<Vector3> treeStumps, List<Vector4> rockpos, float health, int numLogs, bool hasBackpack, Vector3 position, string name, float score, float fireStrength, int fireLogs){
        for (int i = 0; i < treepos.Count; i++){
            float[] f = {treepos[i].x, treepos[i].y, treepos[i].z, treepos[i].w};
            this.treepos.Add(f);
        }
        for (int i = 0; i < rockpos.Count; i++){
            float[] f = {rockpos[i].x, rockpos[i].y, rockpos[i].z, rockpos[i].w};
            this.rockpos.Add(f);
        }
        for (int i = 0; i < treeStumps.Count; i++){
            float[] f = {treeStumps[i].x, treeStumps[i].y, treeStumps[i].z};
            this.treeStumps.Add(f);
        }
        this.health = health;
        this.numLogs = numLogs;
        this.hasBackpack = hasBackpack;
        float[] pos = {position.x, position.y, position.z};
        this.position = pos;
        this.name = name;
        this.score = score;
        this.fireStrength = fireStrength;
        this.fireLogs = fireLogs;
    }

    public List<Vector4> getTreepos(){
        List<Vector4> pos = new List<Vector4>();

        for (int i = 0; i < treepos.Count; i++){
            pos.Add(new Vector4(treepos[i][0], treepos[i][1], treepos[i][2], treepos[i][3]));
        }

        return pos;
    }

    public List<Vector3> getTreeStumppos(){
        List<Vector3> pos = new List<Vector3>();

        for (int i = 0; i < treeStumps.Count; i++){
            pos.Add(new Vector4(treeStumps[i][0], treeStumps[i][1], treeStumps[i][2]));
        }

        return pos;
    }

    public List<Vector4> getRockpos(){
        List<Vector4> pos = new List<Vector4>();

        for (int i = 0; i < rockpos.Count; i++){
            pos.Add(new Vector4(rockpos[i][0], rockpos[i][1], rockpos[i][2], rockpos[i][3]));
        }

        return pos;
    }

}
