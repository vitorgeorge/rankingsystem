using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MBR_Class;
using UnityEngine.Networking;
using MagicClub_Class;

public class scriptRank : MonoBehaviour
{
    [Header("IMAGENS RANK")]
    public Sprite primeiroLugar;
    public Sprite segundoLugar;
    public Sprite terceiroLugar;
    public Sprite colocacoes;
    public Sprite faixaRank;

    [Header("MENU NAVEGA��O")]
    public GameObject[] abasMenu;
    public GameObject[] botoesMenu;
    public GameObject[] botoesMenuClicado;
    public Sprite abaClicada;
    public Sprite abaNeutra;
    Transform posiMenuNacional;

    [Header("ALUNO LOGADO")]
    public Text xpAlunoLogado;
    public Text moedasAlunoLogado;
    public Text estrelasAlunologado;
    public Image avatarJogado;
    public Text textAvatarJogado;
    public Text textPontAvatarJogado;
    public Sprite[] personagemSelecionadoAluno;
    public string[] jogoSelecionado;
    [Header("ABA RANK NACIONAL")]
    public GameObject contentAlunos;
    public GameObject aluno;
    [Header("ALUNO LOGADO RANK NACIONAL")]
    public Text xpAlunoLogadoNacional;
    public Text posiAlunoLogadoNacional;
    public Text nomeAlunoLogadoNacional;
    int TotalAlunos = 0;

    [Header("ABA RANK ESCOLA")]
    public GameObject contentAlunosEscola;

    [Header("ALUNO LOGADO RANK ESCOLA")]
    public Text xpAlunoLogadoEscola;
    public Text posiAlunoLogadoEscola;
    public Text nomeAlunoLogadoEscola;

    [Header("ABA RANK TURMA")]
    public GameObject contentAlunosTurma;

    [Header("ALUNO LOGADO RANK TURMA")]
    public Text xpAlunoLogadoTurma;
    public Text posiAlunoLogadoTurma;
    public Text nomeAlunoLogadoTurma;

    public List<DataAlunos> dadosAlunos = new List<DataAlunos>();
    [SerializeField] public static DataAlunos DataAlunos;
    [SerializeField] public static AlunosRank AlunosRank;

    // Start is called before the first frame update
    void Start()
    {

        clickMenu(0);
        StartCoroutine(retornaDadosLogados());

        retornaDadosAlunoLogado();
        Main.RetornaIDAvatar();
        StartCoroutine(MagicClub.TocarMusica(Resources.Load<AudioClip>("Musicas/rank"), true));
        //0 WILL
        //1 KIN
        //2 IRIS
        //3 MYKE
        //4 SAM
        //StartCoroutine(SERVER.GravaScore("GravaScore", int.Parse(Main.RetornaIdAluno()), int.Parse(Main.RetornaCodigoAluno()), Lista[0].idPergunta, pontos));
    }
    IEnumerator retornaDadosLogados()
    {
        MBR.CarregaTelaLoading();
        StartCoroutine(retornaRankingNacional());
        StartCoroutine(retornaRankingEscola());
        StartCoroutine(retornaRankingTurma());
        yield return new WaitForSeconds(1f);
        MBR.RemoveTelaLoading();
        //foreach
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void fechaRank()
    {
        MBR.CarregaTelaLoading();
        MBR.CarregaCena("Main");
        MBR.RemoveTelaLoading();
    }
    public void clickMenu(int clicado)
    {
        for(int i = 0; i < 4; i++)
        {
            abasMenu[i].SetActive(false);
            //botoesMenu[i].SetActive(false);
            botoesMenuClicado[i].SetActive(false);
           // botoesMenu[i].GetComponent<Image>().sprite = abaNeutra;
        }
        if (clicado == 0)
        {
            abasMenu[0].SetActive(true);
            botoesMenu[0].SetActive(true);
            botoesMenuClicado[0].SetActive(true);
        }
        if(clicado == 1)
        {
            abasMenu[1].SetActive(true);
            botoesMenu[1].SetActive(true);
            botoesMenuClicado[1].SetActive(true);
        }
        if (clicado == 2)
        {
            abasMenu[2].SetActive(true);
            botoesMenu[2].SetActive(true);
            botoesMenuClicado[2].SetActive(true);
        }
        if (clicado == 3)
        {
            abasMenu[3].SetActive(true);
            botoesMenu[3].SetActive(true);
            botoesMenuClicado[3].SetActive(true);
        }
    }
    public void ativaMenu()
    {
        //    float menuNacionalPosy = menuNacional.transform.position.y + 50;
        //    float menuNacionalPosx = menuNacional.transform.position.x;
        //    Vector3 novaPosi = new Vector3(menuNacionalPosx, menuNacionalPosy, 0);
        //    menuNacional.transform.position = novaPosi;
        //menuNacional.GetComponent<Transform>().position.y = new
    }
    public void retornaDadosAlunoLogado()
    {
        xpAlunoLogado.text = Aluno.RetornaXP().ToString();
        moedasAlunoLogado.text = Aluno.RetornaMoedas().ToString();
        estrelasAlunologado.text = Aluno.RetornaEstrelas().ToString();
        avatarJogado.sprite = personagemSelecionadoAluno[Main.RetornaIDAvatar()];
        textAvatarJogado.text = jogoSelecionado[Main.RetornaIDAvatar()];
        textPontAvatarJogado.text = MagicClub.RetornaPontuacaoGameAcao().ToString();
    }

    //PEGA RANKING NACIONAL --------------------
    public IEnumerator retornaRankingNacional()
    {
        print("retorna ranking nacional: ================================");
        WWWForm form = new WWWForm();
        form.AddField("Acao", "RankingNacional");
        form.AddField("IdAluno", Main.RetornaIdAluno());
        form.AddField("Token", "9e9ec316f4276b30a44986717d1a62400bff71e1");

        UnityWebRequest www = UnityWebRequest.Post(SERVER.urlPHP, form);
        int page = 0;
        string[] pages;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("ERRO AO ENVIAR DADOS PARA GRAVA��O");
            Debug.Log(www.error);
        }
        else
        {
            pages = SERVER.urlPHP.Split('/');
            page = pages.Length | 1;
            //DataAlunos = JsonUtility.FromJson<DataAlunos>(www.downloadHandler.text);
            AlunosRank = JsonUtility.FromJson<AlunosRank>(www.downloadHandler.text);
            dadosAlunos = new List<DataAlunos>();
             for (int a = 0; a < AlunosRank.Data.Count; a++)
             {
                dadosAlunos.Add(new DataAlunos()
                {
                    IdAluno = AlunosRank.Data[a].IdAluno,
                    Nome = AlunosRank.Data[a].Nome,
                    Classe = AlunosRank.Data[a].Classe,
                    XP = AlunosRank.Data[a].XP,
                    Posicao = AlunosRank.Data[a].Posicao
                });
                //for (int b = 0; b < TotalAlunos; b++)
                //{
                GameObject novoAluno = Instantiate(aluno, new Vector3(0, 0, 0), Quaternion.identity, contentAlunos.transform);
                Text[] infAlunos;
                Image[] posiAluno;
                posiAluno = novoAluno.GetComponentsInChildren<Image>();
                infAlunos = novoAluno.GetComponentsInChildren<Text>();
                posiAluno[1].GetComponentInChildren<Text>().text = AlunosRank.Data[a].Posicao;
                if (PlayerPrefs.GetString("ID_DO_ALUNO") == AlunosRank.Data[a].IdAluno)
                {
                    posiAluno[0].GetComponent<Image>().sprite = faixaRank;
                    posiAlunoLogadoNacional.text = AlunosRank.Data[a].Posicao;
                    nomeAlunoLogadoNacional.text = AlunosRank.Data[a].Nome;
                    xpAlunoLogadoNacional.text = AlunosRank.Data[a].XP;
                }
                if(AlunosRank.Data[a].Posicao == "1")
                {
                    posiAluno[1].GetComponent<Image>().sprite = primeiroLugar;
                    infAlunos[0].color = new Color32(255, 135, 0, 255);
                }
                else if (AlunosRank.Data[a].Posicao == "2")
                {
                    posiAluno[1].GetComponent<Image>().sprite = segundoLugar;
                    infAlunos[0].color = new Color32(120, 120, 120, 255);
                }
                else if (AlunosRank.Data[a].Posicao == "3")
                {
                    posiAluno[1].GetComponent<Image>().sprite = terceiroLugar;
                    infAlunos[0].color = new Color32(157, 52, 0, 255);
                }
                if (AlunosRank.Data[a].Posicao == "4")
                {
                     posiAluno[1].GetComponent<Image>().sprite = colocacoes;
                }
                infAlunos[0].text = AlunosRank.Data[a].Posicao;
                infAlunos[1].text = AlunosRank.Data[a].Nome;
                infAlunos[2].text = AlunosRank.Data[a].XP;
                TotalAlunos++;
            }
        }
    }
    //PEGA RANK ESCOLA -------------------------
    public IEnumerator retornaRankingEscola()
    {
        print("retorna ranking escola: ================================");
        WWWForm form = new WWWForm();
        form.AddField("Acao", "RankingEscola");
        form.AddField("IdAluno", Main.RetornaIdAluno());
        form.AddField("Token", "9e9ec316f4276b30a44986717d1a62400bff71e1");

        UnityWebRequest www = UnityWebRequest.Post(SERVER.urlPHP, form);
        int page = 0;
        string[] pages;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("ERRO AO ENVIAR DADOS PARA GRAVA��O");
            Debug.Log(www.error);
        }
        else
        {
            pages = SERVER.urlPHP.Split('/');
            page = pages.Length | 1;
            //DataAlunos = JsonUtility.FromJson<DataAlunos>(www.downloadHandler.text);
            AlunosRank = JsonUtility.FromJson<AlunosRank>(www.downloadHandler.text);
            dadosAlunos = new List<DataAlunos>();
            for (int a = 0; a < AlunosRank.Data.Count; a++)
            {
                dadosAlunos.Add(new DataAlunos()
                {
                    IdAluno = AlunosRank.Data[a].IdAluno,
                    Nome = AlunosRank.Data[a].Nome,
                    Classe = AlunosRank.Data[a].Classe,
                    XP = AlunosRank.Data[a].XP,
                    Posicao = AlunosRank.Data[a].Posicao
                });
                //for (int b = 0; b < TotalAlunos; b++)
                //{
                GameObject novoAluno = Instantiate(aluno, new Vector3(0, 0, 0), Quaternion.identity, contentAlunosEscola.transform);
                Text[] infAlunos;
                Image[] posiAluno;
                posiAluno = novoAluno.GetComponentsInChildren<Image>();
                infAlunos = novoAluno.GetComponentsInChildren<Text>();
                posiAluno[1].GetComponentInChildren<Text>().text = AlunosRank.Data[a].Posicao;
                if (PlayerPrefs.GetString("ID_DO_ALUNO") == AlunosRank.Data[a].IdAluno)
                {
                    posiAluno[0].GetComponent<Image>().sprite = faixaRank;
                    posiAlunoLogadoEscola.text = AlunosRank.Data[a].Posicao;
                    nomeAlunoLogadoEscola.text = AlunosRank.Data[a].Nome;
                    xpAlunoLogadoEscola.text = AlunosRank.Data[a].XP;
                }
                if (AlunosRank.Data[a].Posicao == "1")
                {
                    posiAluno[1].GetComponent<Image>().sprite = primeiroLugar;
                    infAlunos[0].color = new Color32(255, 135, 0, 255);
                }
                else if (AlunosRank.Data[a].Posicao == "2")
                {
                    posiAluno[1].GetComponent<Image>().sprite = segundoLugar;
                    infAlunos[0].color = new Color32(120, 120, 120, 255);
                }
                else if (AlunosRank.Data[a].Posicao == "3")
                {
                    posiAluno[1].GetComponent<Image>().sprite = terceiroLugar;
                    infAlunos[0].color = new Color32(157, 52, 0, 255);
                }
                if (AlunosRank.Data[a].Posicao == "4")
                {
                    posiAluno[1].GetComponent<Image>().sprite = colocacoes;
                }
                infAlunos[0].text = AlunosRank.Data[a].Posicao;
                infAlunos[1].text = AlunosRank.Data[a].Nome;
                infAlunos[2].text = AlunosRank.Data[a].XP;
                TotalAlunos++;
            }
        }
    }
    //PEGA RANK TURMA --------------------------
    public IEnumerator retornaRankingTurma()
    {
        print("retorna ranking escola: ================================");
        WWWForm form = new WWWForm();
        form.AddField("Acao", "RankingTurma");
        form.AddField("IdAluno", Main.RetornaIdAluno());
        form.AddField("Token", "9e9ec316f4276b30a44986717d1a62400bff71e1");

        UnityWebRequest www = UnityWebRequest.Post(SERVER.urlPHP, form);
        int page = 0;
        string[] pages;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("ERRO AO ENVIAR DADOS PARA GRAVA��O");
            Debug.Log(www.error);
        }
        else
        {
            pages = SERVER.urlPHP.Split('/');
            page = pages.Length | 1;
            //DataAlunos = JsonUtility.FromJson<DataAlunos>(www.downloadHandler.text);
            AlunosRank = JsonUtility.FromJson<AlunosRank>(www.downloadHandler.text);
            dadosAlunos = new List<DataAlunos>();
            for (int a = 0; a < AlunosRank.Data.Count; a++)
            {
                dadosAlunos.Add(new DataAlunos()
                {
                    IdAluno = AlunosRank.Data[a].IdAluno,
                    Nome = AlunosRank.Data[a].Nome,
                    Classe = AlunosRank.Data[a].Classe,
                    XP = AlunosRank.Data[a].XP,
                    Posicao = AlunosRank.Data[a].Posicao
                });
                //for (int b = 0; b < TotalAlunos; b++)
                //{
                GameObject novoAluno = Instantiate(aluno, new Vector3(0, 0, 0), Quaternion.identity, contentAlunosTurma.transform);
                Text[] infAlunos;
                Image[] posiAluno;
                posiAluno = novoAluno.GetComponentsInChildren<Image>();
                infAlunos = novoAluno.GetComponentsInChildren<Text>();
                posiAluno[1].GetComponentInChildren<Text>().text = AlunosRank.Data[a].Posicao;
                if (PlayerPrefs.GetString("ID_DO_ALUNO") == AlunosRank.Data[a].IdAluno)
                {
                    posiAluno[0].GetComponent<Image>().sprite = faixaRank;
                    posiAlunoLogadoTurma.text = AlunosRank.Data[a].Posicao;
                    nomeAlunoLogadoTurma.text = AlunosRank.Data[a].Nome;
                    xpAlunoLogadoTurma.text = AlunosRank.Data[a].XP;
                }
                if (AlunosRank.Data[a].Posicao == "1")
                {
                    posiAluno[1].GetComponent<Image>().sprite = primeiroLugar;
                    infAlunos[0].color = new Color32(255, 135, 0, 255);
                }
                else if (AlunosRank.Data[a].Posicao == "2")
                {
                    posiAluno[1].GetComponent<Image>().sprite = segundoLugar;
                    infAlunos[0].color = new Color32(120, 120, 120, 255);
                }
                else if (AlunosRank.Data[a].Posicao == "3")
                {
                    posiAluno[1].GetComponent<Image>().sprite = terceiroLugar;
                    infAlunos[0].color = new Color32(157, 52, 0, 255);
                }
                if (AlunosRank.Data[a].Posicao == "4")
                {
                    posiAluno[1].GetComponent<Image>().sprite = colocacoes;
                }
                infAlunos[0].text = AlunosRank.Data[a].Posicao;
                infAlunos[1].text = AlunosRank.Data[a].Nome;
                infAlunos[2].text = AlunosRank.Data[a].XP;
                TotalAlunos++;
            }
        }
    }

}

[System.Serializable]
public class AlunosRank
{
    public List<DataAlunos> Data;
    public string Status;
    public string IdSerie;
    public string IdClasse;
    public string IdBandeira;
}
[System.Serializable]
public class DataAlunos
{   
    
    public string IdAluno;
    public string Nome;
    public string Classe;
    public string XP;
    public string Posicao;
    //public string Data;
}

