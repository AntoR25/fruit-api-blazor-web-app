# Fruit Web App

## Description

Cette application est une interface web qui permet de gérer une liste de fruits via une API.
L’utilisateur doit se connecter avant d’accéder à la liste.

Fonctionnalités principales :

* Connexion utilisateur
* Affichage de la liste des fruits
* Ajouter un fruit
* Modifier un fruit
* Supprimer un fruit

La connexion reste active pendant la session du navigateur.

---

## Technologies utilisées

* **.NET / Blazor**
* **C#**
* **Bootstrap** (interface)
* **HTTP API** pour récupérer et modifier les fruits

---

## Fonctionnement

1. L'utilisateur arrive sur la **page d'accueil**.
2. Il doit entrer un **nom d'utilisateur** et un **mot de passe**.
3. Une fois connecté, la **liste des fruits apparaît**.
4. L'utilisateur peut :

   * Ajouter un fruit
   * Modifier un fruit
   * Supprimer un fruit

---

## Structure du projet

```
Components
 └ Pages
   ├ Home.razor
   ├ Home.razor.cs
   ├ Edit.razor
   ├ Edit.razor.cs
   ├ Add.razor
   └ Delete.razor

Models
 └ FruitModel.cs
```

---

## Lancer le projet

1. Lancer l'API des fruits.
2. Lancer le projet **FruitWebApp**.
3. Ouvrir le navigateur sur :

```
https://localhost:xxxx
```

4. Se connecter pour accéder à la liste des fruits.

---

## REMACLE Antoine

Projet réalisé dans le cadre d'un exercice sur **Blazor et les API REST**.

