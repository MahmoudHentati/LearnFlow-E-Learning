"""
Tests d'Authentification - LearnFlow
Tests Fonctionnels Automatisés avec Playwright

Niveau de Test: Système (E2E)
Technique: Boîte Noire (Partitions d'équivalence, Valeurs limites)

Généré avec l'assistance de Claude (Anthropic)
"""

import pytest
from playwright.sync_api import Page, expect
from pages.login_page import LoginPage
from pages.home_page import HomePage


class TestAuthentification:
    """
    Suite de tests pour l'authentification LearnFlow.
    
    Couvre les scénarios:
    - Connexion valide
    - Connexion invalide (email/mot de passe incorrects)
    - Déconnexion
    - Validation des champs
    """
    
    # ==================== TESTS DE CONNEXION VALIDE ====================
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC001_connexion_valide_admin(self, page: Page):
        """
        TC001: Connexion avec identifiants valides (Admin)
        
        Technique: Boîte noire - Classe d'équivalence valide
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        home_page = HomePage(page)
        
        # Act
        login_page.navigate()
        login_page.login("admin@learnflow.com", "Admin@123")
        
        # Assert
        home_page.expect_dashboard_loaded()
        home_page.expect_user_logged_in()
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC002_connexion_valide_etudiant(self, page: Page):
        """
        TC002: Connexion avec identifiants valides (Étudiant)
        
        Technique: Boîte noire - Classe d'équivalence valide
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("student@learnflow.com", "Student@123")
        
        # Assert
        login_page.expect_login_success()
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC003_connexion_valide_formateur(self, page: Page):
        """
        TC003: Connexion avec identifiants valides (Formateur)
        
        Technique: Boîte noire - Classe d'équivalence valide
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("trainer@learnflow.com", "Trainer@123")
        
        # Assert
        login_page.expect_login_success()
    
    # ==================== TESTS DE CONNEXION INVALIDE ====================
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC004_connexion_mot_de_passe_incorrect(self, page: Page):
        """
        TC004: Connexion avec mot de passe incorrect
        
        Technique: Boîte noire - Classe d'équivalence invalide
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("admin@learnflow.com", "WrongPassword123")
        
        # Assert
        login_page.expect_login_failure()
        assert login_page.is_error_displayed()
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC005_connexion_email_incorrect(self, page: Page):
        """
        TC005: Connexion avec email inexistant
        
        Technique: Boîte noire - Classe d'équivalence invalide
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("inexistant@email.com", "Password123")
        
        # Assert
        login_page.expect_login_failure()
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC006_connexion_champs_vides(self, page: Page):
        """
        TC006: Connexion avec champs vides
        
        Technique: Boîte noire - Valeurs limites (vide)
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("", "")
        
        # Assert
        # Le formulaire ne devrait pas soumettre ou afficher une erreur
        login_page.expect_on_login_page()
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC007_connexion_email_format_invalide(self, page: Page):
        """
        TC007: Connexion avec format d'email invalide
        
        Technique: Boîte noire - Classe d'équivalence invalide
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("email_sans_arobase", "Password123")
        
        # Assert
        login_page.expect_on_login_page()  # Reste sur la page login
    
    # ==================== TESTS DE DÉCONNEXION ====================
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC008_deconnexion(self, page: Page):
        """
        TC008: Déconnexion après connexion réussie
        
        Technique: Boîte noire - Scénario utilisateur
        Niveau: Test Système
        """
        # Arrange
        login_page = LoginPage(page)
        home_page = HomePage(page)
        
        # Act - Connexion
        login_page.navigate()
        login_page.login_as_valid_user()
        home_page.expect_dashboard_loaded()
        
        # Act - Déconnexion
        home_page.logout_and_verify()
        
        # Assert
        login_page.expect_on_login_page()
    
    # ==================== TESTS DE SÉCURITÉ BASIQUE ====================
    
    @pytest.mark.security
    @pytest.mark.nonfunctional
    def test_TC009_injection_sql_login(self, page: Page):
        """
        TC009: Test injection SQL sur le champ email
        
        Technique: Test de sécurité - Injection SQL
        Niveau: Test Système (Non Fonctionnel)
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("' OR '1'='1' --", "password")
        
        # Assert - Ne doit pas permettre l'accès
        login_page.expect_login_failure()
    
    @pytest.mark.security
    @pytest.mark.nonfunctional
    def test_TC010_xss_login(self, page: Page):
        """
        TC010: Test XSS sur le champ email
        
        Technique: Test de sécurité - Cross-Site Scripting
        Niveau: Test Système (Non Fonctionnel)
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        login_page.login("<script>alert('XSS')</script>", "password")
        
        # Assert - Ne doit pas exécuter le script
        login_page.expect_on_login_page()


# ==================== CONFIGURATION PYTEST ====================

@pytest.fixture(scope="function")
def page(browser):
    """Fixture pour créer une nouvelle page pour chaque test."""
    context = browser.new_context()
    page = context.new_page()
    yield page
    page.close()
    context.close()
