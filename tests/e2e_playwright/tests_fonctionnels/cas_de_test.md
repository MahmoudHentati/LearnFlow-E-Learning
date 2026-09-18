# Cas de Test - LearnFlow

## Informations Générales

| Élément | Détail |
|---------|--------|
| **Projet** | LearnFlow - Plateforme E-Training |
| **Version** | 1.0 |
| **Date** | 13 Décembre 2025 |
| **Équipe** | Groupe de développement |

---

## Légende

| Symbole | Signification |
|---------|---------------|
| ✅ | Passé |
| ❌ | Échoué |
| ⏸️ | Non exécuté |
| 🔄 | En cours |

---

## 1. Tests d'Authentification

### TC001 - Connexion valide Admin

| Champ | Valeur |
|-------|--------|
| **ID** | TC001 |
| **Titre** | Connexion avec identifiants valides (Admin) |
| **Créé par** | Équipe Test |
| **Version** | 1.0 |
| **Niveau** | Système |
| **Technique** | Boîte noire - Partition d'équivalence (classe valide) |
| **Priorité** | Haute |

| Prérequis |
|-----------|
| 1. Application LearnFlow démarrée |
| 2. Accès à un navigateur Chrome |
| 3. Compte admin existant dans la base |

| Jeu de données |
|----------------|
| Email: admin@learnflow.com |
| Mot de passe: Admin@123 |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Naviguer vers /login | Page de connexion affichée | | ⏸️ |
| 2 | Saisir l'email admin | Champ accepte la valeur | | ⏸️ |
| 3 | Saisir le mot de passe | Champ masque le mot de passe | | ⏸️ |
| 4 | Cliquer sur "Connexion" | Redirection vers /dashboard | | ⏸️ |

---

### TC002 - Connexion valide Étudiant

| Champ | Valeur |
|-------|--------|
| **ID** | TC002 |
| **Titre** | Connexion avec identifiants valides (Étudiant) |
| **Niveau** | Système |
| **Technique** | Boîte noire - Partition d'équivalence (classe valide) |

| Jeu de données |
|----------------|
| Email: student@learnflow.com |
| Mot de passe: Student@123 |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Naviguer vers /login | Page de connexion affichée | | ⏸️ |
| 2 | Saisir les identifiants étudiant | Champs acceptent les valeurs | | ⏸️ |
| 3 | Cliquer sur "Connexion" | Redirection vers dashboard étudiant | | ⏸️ |

---

### TC003 - Connexion valide Formateur

| Champ | Valeur |
|-------|--------|
| **ID** | TC003 |
| **Titre** | Connexion avec identifiants valides (Formateur) |
| **Niveau** | Système |
| **Technique** | Boîte noire - Partition d'équivalence (classe valide) |

| Jeu de données |
|----------------|
| Email: trainer@learnflow.com |
| Mot de passe: Trainer@123 |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Naviguer vers /login | Page de connexion affichée | | ⏸️ |
| 2 | Saisir les identifiants formateur | Champs acceptent les valeurs | | ⏸️ |
| 3 | Cliquer sur "Connexion" | Redirection vers dashboard formateur | | ⏸️ |

---

### TC004 - Connexion mot de passe incorrect

| Champ | Valeur |
|-------|--------|
| **ID** | TC004 |
| **Titre** | Connexion avec mot de passe incorrect |
| **Niveau** | Système |
| **Technique** | Boîte noire - Partition d'équivalence (classe invalide) |
| **Type** | Test de confirmation |

| Jeu de données |
|----------------|
| Email: admin@learnflow.com |
| Mot de passe: WrongPassword123 |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Naviguer vers /login | Page de connexion affichée | | ⏸️ |
| 2 | Saisir email valide | Champ accepte la valeur | | ⏸️ |
| 3 | Saisir mot de passe incorrect | Champ accepte la valeur | | ⏸️ |
| 4 | Cliquer sur "Connexion" | Message "Mot de passe incorrect" | | ⏸️ |

---

### TC005 - Connexion email inexistant

| Champ | Valeur |
|-------|--------|
| **ID** | TC005 |
| **Titre** | Connexion avec email inexistant |
| **Niveau** | Système |
| **Technique** | Boîte noire - Partition d'équivalence (classe invalide) |

| Jeu de données |
|----------------|
| Email: inexistant@email.com |
| Mot de passe: Password123 |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Naviguer vers /login | Page de connexion affichée | | ⏸️ |
| 2 | Saisir email inexistant | Champ accepte la valeur | | ⏸️ |
| 3 | Cliquer sur "Connexion" | Message "Utilisateur non trouvé" | | ⏸️ |

---

### TC006 - Connexion champs vides

| Champ | Valeur |
|-------|--------|
| **ID** | TC006 |
| **Titre** | Connexion avec champs vides |
| **Niveau** | Système |
| **Technique** | Boîte noire - Valeurs limites (vide) |

| Jeu de données |
|----------------|
| Email: (vide) |
| Mot de passe: (vide) |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Naviguer vers /login | Page de connexion affichée | | ⏸️ |
| 2 | Laisser les champs vides | Champs vides | | ⏸️ |
| 3 | Cliquer sur "Connexion" | Validation: "Champs obligatoires" | | ⏸️ |

---

### TC007 - Format email invalide

| Champ | Valeur |
|-------|--------|
| **ID** | TC007 |
| **Titre** | Connexion avec format email invalide |
| **Niveau** | Système |
| **Technique** | Boîte noire - Partition d'équivalence (format invalide) |

| Jeu de données |
|----------------|
| Email: email_sans_arobase |
| Mot de passe: Password123 |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Saisir email sans @ | Champ accepte la valeur | | ⏸️ |
| 2 | Cliquer sur "Connexion" | Validation: "Format email invalide" | | ⏸️ |

---

### TC008 - Déconnexion

| Champ | Valeur |
|-------|--------|
| **ID** | TC008 |
| **Titre** | Déconnexion après connexion réussie |
| **Niveau** | Système |
| **Technique** | Boîte noire - Scénario utilisateur |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Se connecter avec compte valide | Redirection vers dashboard | | ⏸️ |
| 2 | Cliquer sur "Déconnexion" | Bouton visible et cliquable | | ⏸️ |
| 3 | Confirmer la déconnexion | Redirection vers /login | | ⏸️ |

---

## 2. Tests de Sécurité

### TC009 - Injection SQL

| Champ | Valeur |
|-------|--------|
| **ID** | TC009 |
| **Titre** | Test injection SQL sur login |
| **Niveau** | Système |
| **Technique** | Test de sécurité - Injection |
| **Type** | Non Fonctionnel |

| Jeu de données |
|----------------|
| Email: ' OR '1'='1' -- |
| Mot de passe: password |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Saisir payload SQL injection | Champ accepte la valeur | | ⏸️ |
| 2 | Cliquer sur "Connexion" | Connexion refusée, pas d'accès | | ⏸️ |

---

### TC010 - Cross-Site Scripting (XSS)

| Champ | Valeur |
|-------|--------|
| **ID** | TC010 |
| **Titre** | Test XSS sur login |
| **Niveau** | Système |
| **Technique** | Test de sécurité - XSS |
| **Type** | Non Fonctionnel |

| Jeu de données |
|----------------|
| Email: \<script\>alert('XSS')\</script\> |
| Mot de passe: password |

| # | Étape | Résultat Attendu | Résultat Réel | Statut |
|---|-------|------------------|---------------|--------|
| 1 | Saisir payload XSS | Champ accepte/échappe la valeur | | ⏸️ |
| 2 | Cliquer sur "Connexion" | Pas d'exécution de script | | ⏸️ |

---

## 3. Tests de Navigation

### TC011-TC015 - Navigation

| ID | Titre | Niveau | Technique | Statut |
|----|-------|--------|-----------|--------|
| TC011 | Navigation vers formations | Système | Boîte noire | ⏸️ |
| TC012 | Navigation vers étudiants | Système | Boîte noire | ⏸️ |
| TC013 | Navigation vers profil | Système | Boîte noire | ⏸️ |
| TC014 | Menu navigation visible | Système | Boîte noire | ⏸️ |
| TC015 | Retour au dashboard | Système | Boîte noire | ⏸️ |

---

## 4. Tests de Contrôle d'Accès

### TC016-TC018 - Contrôle d'Accès

| ID | Titre | Niveau | Technique | Statut |
|----|-------|--------|-----------|--------|
| TC016 | Accès dashboard sans auth | Système | Sécurité | ⏸️ |
| TC017 | Accès formations sans auth | Système | Sécurité | ⏸️ |
| TC018 | Accès admin avec rôle étudiant | Système | Sécurité | ⏸️ |

---

## 5. Tests de Performance

### TC019-TC022 - Performance

| ID | Titre | Seuil | Type | Statut |
|----|-------|-------|------|--------|
| TC019 | Temps chargement login | < 3s | Performance | ⏸️ |
| TC020 | Temps connexion | < 2s | Performance | ⏸️ |
| TC021 | Temps chargement dashboard | < 3s | Performance | ⏸️ |
| TC022 | Temps navigation pages | < 3s | Performance | ⏸️ |

---

## 6. Tests de Compatibilité

### TC023-TC025 - Navigateurs

| ID | Titre | Navigateur | Statut |
|----|-------|------------|--------|
| TC023 | Compatibilité Chromium | Chrome | ⏸️ |
| TC024 | Compatibilité Firefox | Firefox | ⏸️ |
| TC025 | Compatibilité WebKit | Safari | ⏸️ |

---

## 7. Tests d'Ergonomie

### TC026-TC028 - Responsive Design

| ID | Titre | Résolution | Statut |
|----|-------|------------|--------|
| TC026 | Responsive Mobile | 375x667 | ⏸️ |
| TC027 | Responsive Tablette | 768x1024 | ⏸️ |
| TC028 | Responsive Desktop | 1920x1080 | ⏸️ |

---

## Résumé des Cas de Test

| Catégorie | Nombre | Technique |
|-----------|--------|-----------|
| Authentification | 8 | Boîte noire (Équivalence, Limites) |
| Sécurité | 2 | Test de sécurité |
| Navigation | 5 | Boîte noire |
| Contrôle d'accès | 3 | Test de sécurité |
| Performance | 4 | Test non fonctionnel |
| Compatibilité | 3 | Test non fonctionnel |
| Ergonomie | 3 | Test non fonctionnel |
| **TOTAL** | **28** | |

---

> **Note IA** : Ces cas de test ont été générés avec l'assistance de Claude (Anthropic) en suivant le template fourni dans les guidelines du projet.
