int[,] field = new int[4, 4];
bool[,] revealed = new bool[4, 4];
Random random = new Random();

// předepsaný hodnoty protože se velikost pole nemení
// takže není potřeba vymýšlet algorithmus pro diagonální
int[,,] diagonals = {
    // topleft -> botright
    {{0, 0}, {1, 1}, {2, 2}, {3, 3}},
    // topright -> leftbot
    {{0, 3}, {1, 2}, {2, 1}, {3, 0} }
};



void generate_field_values(){
    for (int i = 0; i < 4; i++){
        for (int j = 0; j < 4; j++){
            field[i, j] = random.Next(1, 100);
            revealed[i, j] = false;
        }
    }
}

void reveal_field(){
    for (int i = 0; i < 4; i++){
        for (int j = 0; j < 4; j++) {
            revealed[i, j] = true;
        }
    }
}

void display_field()
{
    Console.WriteLine("┌────┬────┬────┬────┐");
    for(int i = 0; i < 4; i++){
        for(int j = 0; j < 4; j++) {
            string field_value = "??";
            if(revealed[i, j]) {
                field_value = field[i, j].ToString();
                if(field_value.Length == 1)
                {
                    field_value = " " + field_value;
                }
            }
            if(j == 3){
                Console.Write("│ " + field_value + " │");
            }
            else{
                Console.Write("│ " + field_value + " ");
            }
            
        }
        Console.WriteLine();
        if(i != 3) {
            Console.Write("├────┼────┼────┼────┤");
            Console.WriteLine();
        }

    }
    Console.WriteLine("└────┴────┴────┴────┘");
}


Tuple<bool, int[]> get_input(){
    int[] input_values = Enumerable.Repeat(-1, 8).ToArray();
    Console.Write("Zadejte 4 až 8 hodnot mezi (0-99) separovaný mezerou > ");
    string[] input = Console.ReadLine().Split(" ");
    if(input.Length < 4 || input.Length > 8) {
        Console.WriteLine("Nesprávný počet hodnot! Hodnot musí být mezi 4 až 8");
        // uživatel zadal míň jak 4 nebo víc jak 8 hodnot
        return new Tuple<bool, int[]>(false, input_values);
    }

    int index = 0;
    foreach (string string_value in input){
        try {
            int value = int.Parse(string_value);
            if(value < 0 || value > 99){
                Console.WriteLine("Hodnota můsí být mezi 0 až 99!");
            }
            input_values[index++] = value;
        }catch(Exception ex){
            Console.WriteLine("Hodnota můsí být číslo");
        }
    }
    return new Tuple<bool, int[]>(true, input_values);
}

void clear_console()
{
    Thread.Sleep(1000);
    Console.Clear();
}


bool fill_and_eval(int[] values) {
    // doplní hodnoty zadané argumentem
    int revealed_fields = 0;
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            if (values.Contains(field[i, j]) && !revealed[i, j])
            {
                revealed[i, j] = true;
                revealed_fields++;
            }
        }
    }
    Console.WriteLine("Bylo odhaleno {0} hodnot", revealed_fields);

    bool got_bingo = false;

    for (int i = 0; i < 4; i++)
    {
        int in_row = 0;
        for (int j = 0; j < 4; j++)
        {
            // jelikož pole je 4x4 tak in_row se nemusí resetovat když pole neni revealed
            if (revealed[i, j])
            {
                in_row++;
            }
        }
        if(in_row == 4){
            got_bingo = true;
            break;
        }
    }

    if (got_bingo) {
        return got_bingo;
    }

    for(int diagonal_index = 0; diagonal_index < 2; diagonal_index++){
        int in_diagonal = 0;
        for(int i = 0; i < 4; i++)
        {
            int y = diagonals[diagonal_index, i, 0];
            int x = diagonals[diagonal_index, i, 1];

            if (revealed[y, x])
            {
                in_diagonal++;
            }
        }
        if (in_diagonal == 4)
        {
            got_bingo = true;
            break;
        }
    }

    return got_bingo;
}


generate_field_values();

while (true){
    display_field();
    Tuple<bool, int[]> tuple = get_input();
    if (tuple.Item1)
    {
        clear_console();
        bool got_bingo = fill_and_eval(tuple.Item2);
        if (got_bingo) {
            Console.WriteLine("BINGO! 4 hodnoty vedle sebe nebo diagonálně byly uhodnuty");
            display_field();
            Console.WriteLine("Herní pole vypadalo");
            reveal_field();
            display_field();
            Console.ReadKey();
            break;
        }
    }
    clear_console();
}
