using System;
using System.Collections.Generic;
using UnityEngine;

public class FlipFlop
{
    bool valor;
    public bool autoToggle = false;

    public bool Value
    {
        get
        {
            if ( autoToggle )
            {
                autoToggle = false;
                valor = !valor;
            }
            return valor;
        }
        private set
        {
        }
    }

    public FlipFlop( bool estadoInicial = false )
    {
        valor = estadoInicial;
    }

    public void Toggle( )
    {
        valor = !valor;
    }

    public override string ToString( )
    {
        return valor ? "on" : "off";
    }
}

public class Counter
{
    int valor;
    bool descendente;
    public bool autoTick = false;

    public int Value
    {
        get
        {
            if ( autoTick )
            {
                autoTick = false;
                if ( descendente )
                {
                    valor++;
                }
                else
                {
                    valor--;
                }
            }
            return valor;
        }
        private set
        {
        }
    }

    public Counter( int valor = 0, bool descendente = false )
    {
        this.valor = valor;
        this.descendente = descendente;
    }

    public Counter( bool descendente )
    {
        this.descendente = descendente;
    }

    public Counter Reset( int valor = 0 )
    {
        autoTick = false;
        this.valor = valor;
        return this;
    }

    public Counter Tick( )
    {
        if ( descendente )
        {
            valor--;
        }
        else
        {
            valor++;
        }
        return this;
    }

    public override string ToString( )
    {
        return valor.ToString();
    }
}

public class Show : MonoBehaviour
{

    public class Data
    {
        public DataType dataType;
        public string label;
        public object valor;

        public Data( DataType dataType, string label, object value )
        {
            this.dataType = dataType;
            this.label = label;
            this.valor = value;
        }
    }

    [Flags]
    public enum Modifiers
    {
        NonPersistent,  // Hacer un sistema para que el valor desaparezca tras un tiempo configurable
        Max,            // Que muestre el valor máximo que ha tenido la clave
        Min,            // Que muestre el valor minimo que ha tenido la clave
        Average,        // Que muestre el valor promedio que ha tenido la clave
    }

    public enum DataType
    {
        String,
        Integer,
        Float,   // Podría configurarse el número de decimales a mostrar
        Double,  // Podría configurarse el número de decimales a mostrar
        Bool,    // Puede ser un checkbox
        FlipFlop,// Cambia de estado cada vez que se invoca
        Counter, // Una especie de iterador
        Vector2,
        Vector3,

        Light,
        Color   // Podría ser un pequeño rectángulo de ese color
    }

    public enum Esquina
    {
        SuperiorIzquierda, SuperiorDerecha, InferiorIzquierda, InferiorDerecha // una opción futura podría ser custom
    }

    public struct Settings
    {
        public Esquina esquina;
        public Rect background;

        // OPCIONES A NIVEL DE CONFIGURACIÓN GLOBAL QUE PODRÍA HABER
        /*
        bool trimID = true; // si false, considera distintas keys "Balas", "Balas  " y "   Balas"
        bool caseSensitiveID=true; // si false, considera distintas keys "balas", "BALAS" y "Balas"
        string separadorLabels=": ";
        */
    }
    Settings settings;

    bool visible = true; // Si el panel es visible o no

    Dictionary<string, Data> datos = new Dictionary<string, Data>(); // El array de líneas a imprimir

    float ssx = Screen.width;
    float ssy = Screen.height;
    float padding = 10;
    private GUIStyle guiStyle = new GUIStyle( );
    public int textSize = 25;
    public Color textColor = Color.white;
    public Color backgroundColor = Color.black;
    public int panelWidth = 400;


    // Permite borrar una clave de cualquier tipo con Show.Remove
    public static void Remove( string id )
    {
        if ( Instance.datos.ContainsKey( id ) )
        {
            Instance.datos.Remove( id );
            Instance.settings.background.height -= Instance.textSize;
        }
    }

    // Show.String("Enemigo alcanzado");
    // Show.String("Lenguaje", "Español");
    public static void String( string id, string mensaje = "" )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
        }
        Instance.datos [ id ] = new Data( DataType.String, id, mensaje );
    }

    // Show.Bool("Visible", false);
    public static void Bool( string id, bool mensaje )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
        }
        Instance.datos [ id ] = new Data( DataType.Bool, id, mensaje );
    }

    // Show.Int("Balas restantes", 47);
    public static void Int( string id, int mensaje )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
        }
        Instance.datos [ id ] = new Data( DataType.Integer, id, mensaje );
    }

    // Show.Float("Distancia objetivo", 47.45f);
    public static void Float( string id, float mensaje )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
        }
        Instance.datos [ id ] = new Data( DataType.Float, id, mensaje );
    }

    // Show.Double("Distancia Colision", 476.454f);
    public static void Double( string id, float mensaje )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
        }
        Instance.datos [ id ] = new Data( DataType.Double, id, mensaje );
    }

    // Show.Vector2("Posición enemigo", new Vector2(23, 65));
    public static void Vector2( string id, Vector2 mensaje )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
        }
        Instance.datos [ id ] = new Data( DataType.Vector2, id, mensaje );
    }

    // Show.Vector3("Posición enemigo", new Vector3(23, 65, 23));
    public static void Vector3( string id, Vector3 mensaje )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
        }
        Instance.datos [ id ] = new Data( DataType.Vector3, id, mensaje );
    }

    // Show.FlipFlop("Estado");
    public static FlipFlop FlipFlop( string id, bool valorInicial=false )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
            Instance.datos [ id ] = new Data( DataType.Counter, id, new FlipFlop( valorInicial ) );
        }
        else
        {
            ( (FlipFlop) Instance.datos [ id ].valor ).Toggle();
            ( (FlipFlop) Instance.datos [ id ].valor ).autoToggle = true;
        }

        return Instance.datos [ id ].valor as FlipFlop;

    }

    // Show.Counter("Golpeos totales");
    // Debe haber forma de resetear el contador, de establecerle un nuevo valor, de hacer que vaya hacia atrás, ...
    // ver si con métodos diferenciados, o pasando modificadores, o como
    public static void Counter( string id, bool reverse )
    {
        Counter( id, 0, reverse );
    }

    public static Counter Counter( string id, int valorInicial = 0, bool reverse = false )
    {
        if ( !Instance.datos.ContainsKey( id ) )
        {
            Instance.settings.background.height += Instance.textSize;
            Instance.datos [ id ] = new Data( DataType.Counter, id, new Counter( valorInicial, reverse ) );
        }
        else
        {
            Instance.datos [ id ].valor = ( (Counter) Instance.datos [ id ].valor ).Tick();
            ( (Counter) Instance.datos [ id ].valor ).autoTick = true;
        }
        return Instance.datos [ id ].valor as Counter;
    }

    private Texture2D MakeTex ( int width, int height, Color col )
    {
        Color [ ] pix = new Color [ width * height ];
        for ( int i = 0 ; i < pix.Length ; ++i )
        {
            pix [ i ] = col;
        }
        Texture2D result = new Texture2D( width, height );
        result.SetPixels( pix );
        result.Apply( );
        return result;
    }

    private void Start( )
    {
        guiStyle.fontSize = textSize;
        guiStyle.normal.textColor = textColor;
        guiStyle.normal.background = MakeTex( 2, 2, backgroundColor );
        Reposiciona( );
    }

    void Update( )
    {
        if ( Input.GetKeyDown( KeyCode.F12 ) )
            visible = !visible;

        if ( Input.GetKeyDown( KeyCode.Tab ) )
        {
            settings.esquina = ( settings.esquina != Esquina.InferiorDerecha ) ? settings.esquina + 1 : Esquina.SuperiorIzquierda;
            Reposiciona();
        }
    }

    void Reposiciona( )
    {

        settings.background.width = panelWidth;

        settings.background.position = ( settings.esquina == Esquina.SuperiorIzquierda ) ? new Vector2( padding, padding ) :
            ( settings.esquina == Esquina.SuperiorDerecha ) ? new Vector2( ssx - panelWidth - padding, padding ) :
            ( settings.esquina == Esquina.InferiorIzquierda ) ? new Vector2( padding, ssy - settings.background.height - padding ) :
            new Vector2( ssx - panelWidth - padding, ssy - settings.background.height - padding );
    }


    void OnGUI( )
    {
        if ( !visible )
            return;
        if ( datos.Count == 0 )
            return;

  //      GUI.Box( settings.background, "", guiStyle );

        float offset = 0;
        foreach ( KeyValuePair<string, Data> entry in datos )
        {
            Data data = entry.Value;
            if ( data.dataType == DataType.String )
            {
                string separador = "";
                if ( ( (String) data.valor ).Length > 0 )
                    separador = ": ";
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + separador + data.valor as string, guiStyle );
            }
            if ( data.dataType == DataType.Bool )
            {
                bool valor = (bool) data.valor;
                string textoamostrar = ( valor == true ) ? "true" : "false";
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + textoamostrar, guiStyle );
            }
            if ( data.dataType == DataType.Integer )
            {
                int valor = (int) data.valor;
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + valor, guiStyle );
            }
            if ( data.dataType == DataType.Float )
            {
                float valor = (float) data.valor;
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + valor, guiStyle );
            }
            if ( data.dataType == DataType.Double )
            {
                double valor = (double) data.valor;
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + valor, guiStyle );
            }
            if ( data.dataType == DataType.Vector2 )
            {
                Vector2 valor = (Vector2) data.valor;
                string textoamostrar = "X=" + valor.x + ", Y=" + valor.y;
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + textoamostrar, guiStyle );
            }
            if ( data.dataType == DataType.Vector3 )
            {
                Vector3 valor = (Vector3) data.valor;
                string textoamostrar = "X=" + valor.x + ", Y=" + valor.y + ", Z=" + valor.z;
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + textoamostrar, guiStyle );
            }
            if ( data.dataType == DataType.Counter )
            {
                //int valor = (int) data.valor;
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + ( (Counter) data.valor ).ToString(), guiStyle );
            }
            if ( data.dataType == DataType.FlipFlop )
            {
                bool valor = (bool) data.valor;
                data.valor = !valor;
                string textoamostrar = ( valor == true ) ? "on" : "off";
                GUI.Label( new Rect( settings.background.x + 5, settings.background.y + offset, panelWidth - 10, Instance.textSize ), data.label + ": " + textoamostrar, guiStyle );
            }

            offset += Instance.textSize;
        }
    }

    // PATRON SINGLETON. Solo permite una instancia de la clase
    private void Awake( )
    {
        if ( _instance != null && _instance != this )
        {
            Destroy( this.gameObject );
        }
        else
        {
            _instance = this;
        }
    }

    // Patrón Singleton
    private static Show _instance;
    private static Show Instance
    {
        get
        {
            if ( _instance == null )
            {
                _instance = new GameObject( "InGame Debug" ).AddComponent<Show>();
            }

            return _instance;
        }
    }


}
