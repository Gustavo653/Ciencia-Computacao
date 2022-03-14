package com.mycompany.atividades_logica;

import java.util.Scanner;

public class Atividades_Logica {

    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        while (true) {

            System.out.println("Qual atividade você deseja acessar?");
            System.out.println("1 - Média Ponderada");
            System.out.println("2 - Salário Aumento");
            System.out.println("3 - Conversão Unidades");
            int opcao = scan.nextInt();

            if (opcao == 1) {
                int n1, n2, n3, n4, p1, p2, p3, p4;
                double media;
                System.out.println("Insira a nota 1: ");
                n1 = scan.nextInt();
                System.out.println("Insira a peso 1: ");
                p1 = scan.nextInt();
                System.out.println("Insira a nota 2: ");
                n2 = scan.nextInt();
                System.out.println("Insira a peso 2: ");
                p2 = scan.nextInt();
                System.out.println("Insira a nota 3: ");
                n3 = scan.nextInt();
                System.out.println("Insira a peso 3: ");
                p3 = scan.nextInt();
                System.out.println("Insira a nota 4: ");
                n4 = scan.nextInt();
                System.out.println("Insira a peso 4: ");
                p4 = scan.nextInt();

                media = (n1 * p1) + (n2 * p2) + (n3 * p3) + (n4 * p4);
                media /= p1 + p2 + p3 + p4;

                System.out.println("A média deu: " + media);
            } else if (opcao == 2) {

                double salario;
                System.out.println("Insira o salário atual: ");
                salario = scan.nextDouble();
                salario *= 1.25;
                System.out.println("O salário com aumento é: " + salario);

            } else if (opcao == 3) {

                double medida;
                int opcao_medida;
                System.out.println("Insira o a medida em pés:");
                medida = scan.nextDouble();
                System.out.println("Qual conversão você deseja realizar?");
                System.out.println("1 - Polegadas");
                System.out.println("2 - Jardas");
                System.out.println("3 - Milhas");
                opcao_medida = scan.nextInt();
                
                if (opcao_medida == 1) {
                    System.out.println("A conversão de pés para polegadas deu: " + (medida * 12));
                } else if (opcao_medida == 2) {
                    System.out.println("A conversão de pés para jardas deu: " + (medida / 3));
                } else {
                    System.out.println("A conversão de pés para milhas deu: " + ((medida / 3) / 1760));
                }

            } else if (opcao == 4) {

            } else {

            }
        }
    }
}
