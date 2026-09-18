# Matrice de Traçabilité - LearnFlow

## Informations Générales

| Élément | Détail |
|---------|--------|
| **Projet** | LearnFlow - Plateforme E-Training |
| **Version** | 1.0 |
| **Date** | 13 Décembre 2025 |

---

## Légende

| Symbole | Signification |
|---------|---------------|
| ✅ | Couvert et Passé |
| ❌ | Couvert et Échoué |
| ⏸️ | Couvert, Non Exécuté |
| ➖ | Non Applicable |

---

## 1. Traçabilité Exigences → Cas de Test

| ID Exigence | Description Exigence | Cas de Test | Scénario | Couverture |
|-------------|---------------------|-------------|----------|------------|
| **REQ-AUTH-001** | L'utilisateur doit pouvoir se connecter avec email et mot de passe | TC001, TC002, TC003 | Connexion valide | ⏸️ |
| **REQ-AUTH-002** | Le système doit refuser les identifiants incorrects | TC004, TC005 | Connexion invalide | ⏸️ |
| **REQ-AUTH-003** | Les champs de connexion doivent être validés | TC006, TC007 | Validation champs | ⏸️ |
| **REQ-AUTH-004** | L'utilisateur doit pouvoir se déconnecter | TC008 | Déconnexion | ⏸️ |
| **REQ-SEC-001** | Le système doit être protégé contre injection SQL | TC009 | Sécurité SQL | ⏸️ |
| **REQ-SEC-002** | Le système doit être protégé contre XSS | TC010 | Sécurité XSS | ⏸️ |
| **REQ-SEC-003** | Les pages protégées nécessitent authentification | TC016, TC017 | Contrôle accès | ⏸️ |
| **REQ-SEC-004** | Les rôles utilisateurs doivent être respectés | TC018 | Contrôle rôles | ⏸️ |
| **REQ-NAV-001** | L'utilisateur doit pouvoir naviguer entre les sections | TC011, TC012, TC013, TC014, TC015 | Navigation | ⏸️ |
| **REQ-PERF-001** | Les pages doivent charger en moins de 3 secondes | TC019, TC020, TC021, TC022 | Performance | ⏸️ |
| **REQ-COMP-001** | L'application doit fonctionner sur Chrome, Firefox, Safari | TC023, TC024, TC025 | Compatibilité | ⏸️ |
| **REQ-ERG-001** | L'application doit être responsive | TC026, TC027, TC028 | Ergonomie | ⏸️ |

---

## 2. Traçabilité Scénarios → Cas de Test → Résultats

### Scénario 1: Authentification

| ID Scénario | Description | Cas de Test | Niveau | Résultat |
|-------------|-------------|-------------|--------|----------|
| SCN-AUTH-01 | Connexion Admin | TC001 | Système | ⏸️ |
| SCN-AUTH-02 | Connexion Étudiant | TC002 | Système | ⏸️ |
| SCN-AUTH-03 | Connexion Formateur | TC003 | Système | ⏸️ |
| SCN-AUTH-04 | Échec mot de passe | TC004 | Système | ⏸️ |
| SCN-AUTH-05 | Échec email | TC005 | Système | ⏸️ |
| SCN-AUTH-06 | Champs vides | TC006 | Système | ⏸️ |
| SCN-AUTH-07 | Email invalide | TC007 | Système | ⏸️ |
| SCN-AUTH-08 | Déconnexion | TC008 | Système | ⏸️ |

### Scénario 2: Sécurité

| ID Scénario | Description | Cas de Test | Niveau | Résultat |
|-------------|-------------|-------------|--------|----------|
| SCN-SEC-01 | Protection SQL Injection | TC009 | Système | ⏸️ |
| SCN-SEC-02 | Protection XSS | TC010 | Système | ⏸️ |
| SCN-SEC-03 | Accès non autorisé dashboard | TC016 | Système | ⏸️ |
| SCN-SEC-04 | Accès non autorisé formations | TC017 | Système | ⏸️ |
| SCN-SEC-05 | Escalade de privilèges | TC018 | Système | ⏸️ |

### Scénario 3: Navigation

| ID Scénario | Description | Cas de Test | Niveau | Résultat |
|-------------|-------------|-------------|--------|----------|
| SCN-NAV-01 | Accès formations | TC011 | Système | ⏸️ |
| SCN-NAV-02 | Accès étudiants | TC012 | Système | ⏸️ |
| SCN-NAV-03 | Accès profil | TC013 | Système | ⏸️ |
| SCN-NAV-04 | Menu visible | TC014 | Système | ⏸️ |
| SCN-NAV-05 | Retour dashboard | TC015 | Système | ⏸️ |

### Scénario 4: Performance

| ID Scénario | Description | Cas de Test | Niveau | Résultat |
|-------------|-------------|-------------|--------|----------|
| SCN-PERF-01 | Temps chargement login | TC019 | Non Fonctionnel | ⏸️ |
| SCN-PERF-02 | Temps connexion | TC020 | Non Fonctionnel | ⏸️ |
| SCN-PERF-03 | Temps chargement dashboard | TC021 | Non Fonctionnel | ⏸️ |
| SCN-PERF-04 | Temps navigation | TC022 | Non Fonctionnel | ⏸️ |

### Scénario 5: Compatibilité

| ID Scénario | Description | Cas de Test | Niveau | Résultat |
|-------------|-------------|-------------|--------|----------|
| SCN-COMP-01 | Chrome/Chromium | TC023 | Non Fonctionnel | ⏸️ |
| SCN-COMP-02 | Firefox | TC024 | Non Fonctionnel | ⏸️ |
| SCN-COMP-03 | Safari/WebKit | TC025 | Non Fonctionnel | ⏸️ |

### Scénario 6: Ergonomie

| ID Scénario | Description | Cas de Test | Niveau | Résultat |
|-------------|-------------|-------------|--------|----------|
| SCN-ERG-01 | Affichage Mobile | TC026 | Non Fonctionnel | ⏸️ |
| SCN-ERG-02 | Affichage Tablette | TC027 | Non Fonctionnel | ⏸️ |
| SCN-ERG-03 | Affichage Desktop | TC028 | Non Fonctionnel | ⏸️ |

---

## 3. Matrice de Couverture

### Par Type de Test

| Type | Nombre de Tests | Couverture |
|------|-----------------|------------|
| Fonctionnel | 18 | 100% |
| Non Fonctionnel | 10 | 100% |
| **Total** | **28** | **100%** |

### Par Niveau de Test

| Niveau | Nombre de Tests | Couverture |
|--------|-----------------|------------|
| Système (E2E) | 28 | 100% |
| Intégration | - | Via API Tests |
| Unitaire | - | Via .NET Tests |

### Par Technique de Test

| Technique | Nombre de Tests |
|-----------|-----------------|
| Boîte Noire - Équivalence | 8 |
| Boîte Noire - Valeurs Limites | 2 |
| Boîte Noire - Scénario | 6 |
| Test de Sécurité | 5 |
| Test de Performance | 4 |
| Test de Compatibilité | 3 |

---

## 4. Résumé Statistique

| Métrique | Valeur |
|----------|--------|
| Total Exigences | 12 |
| Total Scénarios | 28 |
| Total Cas de Test | 28 |
| Cas Exécutés | 0 |
| Cas Passés | 0 |
| Cas Échoués | 0 |
| Taux de Couverture | 100% |
| Taux de Réussite | - |

---

> **Note IA** : Cette matrice de traçabilité a été générée avec l'assistance de Claude (Anthropic) pour assurer la couverture complète des exigences.
