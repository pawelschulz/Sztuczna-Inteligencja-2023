#include <stdio.h>
#include <time.h>
#include <stdlib.h>

//g(s) = (s-1)^2
//sg* = 1
//g(sg*) = 0 <= g(s)
//snalezy do S

float acc(float sa, float sb, float T)      //acceptance criterium
{
    float delta = sa-sb;
    if (delta <= 0)
        return 1;
    else
        return exp(-delta/T);
}

int random(int min, int max)        // generate random numbers between min and max
{
    int tmp;
    if (max>=min)
        max-= min;
    else
    {
        tmp= min - max;
        min= max;
        max= tmp;
    }
    return max ? (rand() % max + min) : min;
}

// s0 = solution start (-10) ; sf = final (10)
float sa(float s0, float sf)
{
    // global variables
    int T = 1;
    float Tmax = 1394.12;
    float Tmin = 0.0001;
    float alpha = 0.9;

    int M = 100;
    int N = 100;

     for (int i = 0; i < M; i++) {
        for (int j = 0; j < N; j++) {
            sourceArray[i][j] = 'X';
        }
    }  
    float sb = s0;       //sb = the best
    float sa;            //sa = actuall
    int i=0;
    do
    {
        sa = random(s0,sf);     //choose a random solution
        if (acc(sa))
        T *= alpha;     // decreases actuall temp
        i++;
    }while(T > Tmin);   // during temp > temp_min

    return sb;
}
