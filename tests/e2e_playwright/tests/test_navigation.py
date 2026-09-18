"""
Tests de Navigation - LearnFlow
Tests Fonctionnels Automatisés avec Playwright

Niveau de Test: Système (E2E)
Technique: Boîte Noire (Scénarios utilisateur)

Généré avec l'assistance de Claude (Anthropic)
"""

import pytest
from playwright.sync_api import Page, expect
from pages.login_page import LoginPage
from pages.home_page import HomePage
from pages.base_page import BasePage


class TestNavigation:
    """
    Suite de tests pour la navigation LearnFlow.
    
    Couvre les scénarios:
    - Navigation entre pages
    - Accès aux différentes sections
    - Vérification des menus
    """
    
    @pytest.fixture(autouse=True)
    def setup(self, page: Page):
        """Setup: Connexion avant chaque test de navigation."""
        login_page = LoginPage(page)
        login_page.navigate()
        login_page.login_as_valid_user()
        self.page = page
        self.home_page = HomePage(page)
        self.home_page.expect_dashboard_loaded()
    
    # ==================== TESTS DE NAVIGATION ====================
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC011_navigation_vers_formations(self, page: Page):
        """
        TC011: Navigation vers la liste des formations
        
        Technique: Boîte noire - Scénario navigation
        Niveau: Test Système
        """
        # Act
        self.home_page.click_trainings()
        
        # Assert
        expect(page).to_have_url("**/training**")
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC012_navigation_vers_etudiants(self, page: Page):
        """
        TC012: Navigation vers la liste des étudiants
        
        Technique: Boîte noire - Scénario navigation
        Niveau: Test Système
        """
        # Act
        self.home_page.click_students()
        
        # Assert
        expect(page).to_have_url("**/student**")
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC013_navigation_vers_profil(self, page: Page):
        """
        TC013: Navigation vers le profil utilisateur
        
        Technique: Boîte noire - Scénario navigation
        Niveau: Test Système
        """
        # Act
        self.home_page.click_profile()
        
        # Assert
        expect(page).to_have_url("**/profile**")
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC014_menu_navigation_visible(self, page: Page):
        """
        TC014: Vérification visibilité du menu de navigation
        
        Technique: Boîte noire - Vérification UI
        Niveau: Test Système
        """
        # Assert
        assert self.home_page.is_navigation_visible()
    
    @pytest.mark.functional
    @pytest.mark.system
    def test_TC015_retour_dashboard(self, page: Page):
        """
        TC015: Retour au dashboard depuis une autre page
        
        Technique: Boîte noire - Scénario navigation
        Niveau: Test Système
        """
        # Arrange - Aller sur une autre page
        self.home_page.click_trainings()
        expect(page).to_have_url("**/training**")
        
        # Act - Retour au dashboard
        self.home_page.navigate()
        
        # Assert
        self.home_page.expect_dashboard_loaded()


class TestAccessControl:
    """
    Suite de tests pour le contrôle d'accès.
    
    Vérifie que les utilisateurs non authentifiés
    ne peuvent pas accéder aux pages protégées.
    """
    
    @pytest.mark.security
    @pytest.mark.nonfunctional
    def test_TC016_acces_dashboard_sans_auth(self, page: Page):
        """
        TC016: Accès au dashboard sans authentification
        
        Technique: Test de sécurité - Contrôle d'accès
        Niveau: Test Système (Non Fonctionnel)
        """
        # Arrange
        base_page = BasePage(page)
        
        # Act - Tenter d'accéder directement au dashboard
        base_page.navigate_to("dashboard")
        
        # Assert - Doit rediriger vers login
        expect(page).to_have_url("**/login**")
    
    @pytest.mark.security
    @pytest.mark.nonfunctional
    def test_TC017_acces_trainings_sans_auth(self, page: Page):
        """
        TC017: Accès aux formations sans authentification
        
        Technique: Test de sécurité - Contrôle d'accès
        Niveau: Test Système (Non Fonctionnel)
        """
        # Arrange
        base_page = BasePage(page)
        
        # Act
        base_page.navigate_to("training")
        
        # Assert
        expect(page).to_have_url("**/login**")
    
    @pytest.mark.security
    @pytest.mark.nonfunctional
    def test_TC018_acces_admin_avec_role_etudiant(self, page: Page):
        """
        TC018: Accès aux pages admin avec rôle étudiant
        
        Technique: Test de sécurité - Contrôle d'accès par rôle
        Niveau: Test Système (Non Fonctionnel)
        """
        # Arrange
        login_page = LoginPage(page)
        login_page.navigate()
        login_page.login_as_student()
        
        # Act - Tenter d'accéder à une page admin
        page.goto(f"{BasePage.BASE_URL}/admin/users")
        
        # Assert - Doit être refusé ou redirigé
        # Vérifie qu'on n'est pas sur la page admin
        expect(page).not_to_have_url("**/admin/users**")
