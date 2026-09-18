"""
Tests Non Fonctionnels - LearnFlow
Tests de Performance et Compatibilité avec Playwright

Type de Test: Non Fonctionnel
Techniques: Performance, Compatibilité navigateur

Généré avec l'assistance de Claude (Anthropic)
"""

import pytest
import time
from playwright.sync_api import Page, Browser, expect
from pages.login_page import LoginPage
from pages.home_page import HomePage
from pages.base_page import BasePage


class TestPerformance:
    """
    Suite de tests de performance.
    
    Mesure les temps de réponse et les performances
    de l'application LearnFlow.
    """
    
    # Seuils de performance acceptables (en secondes)
    THRESHOLD_PAGE_LOAD = 3.0  # 3 secondes max pour charger une page
    THRESHOLD_LOGIN = 2.0      # 2 secondes max pour se connecter
    THRESHOLD_API_RESPONSE = 1.0  # 1 seconde max pour une réponse API
    
    @pytest.mark.performance
    @pytest.mark.nonfunctional
    def test_TC019_temps_chargement_page_login(self, page: Page):
        """
        TC019: Temps de chargement de la page de connexion
        
        Type: Test de Performance
        Seuil: < 3 secondes
        """
        # Arrange
        login_page = LoginPage(page)
        
        # Act - Mesurer le temps
        start_time = time.time()
        login_page.navigate()
        login_page.wait_for_load()
        end_time = time.time()
        
        # Calculate
        load_time = end_time - start_time
        
        # Assert
        assert load_time < self.THRESHOLD_PAGE_LOAD, \
            f"Page login trop lente: {load_time:.2f}s (seuil: {self.THRESHOLD_PAGE_LOAD}s)"
        
        # Log pour le rapport
        print(f"\n[PERFORMANCE] Page Login: {load_time:.2f}s")
    
    @pytest.mark.performance
    @pytest.mark.nonfunctional
    def test_TC020_temps_connexion(self, page: Page):
        """
        TC020: Temps de réponse pour la connexion
        
        Type: Test de Performance
        Seuil: < 2 secondes
        """
        # Arrange
        login_page = LoginPage(page)
        login_page.navigate()
        
        # Act - Mesurer le temps de connexion
        start_time = time.time()
        login_page.login_as_valid_user()
        page.wait_for_url("**/dashboard**", timeout=10000)
        end_time = time.time()
        
        # Calculate
        login_time = end_time - start_time
        
        # Assert
        assert login_time < self.THRESHOLD_LOGIN, \
            f"Connexion trop lente: {login_time:.2f}s (seuil: {self.THRESHOLD_LOGIN}s)"
        
        print(f"\n[PERFORMANCE] Connexion: {login_time:.2f}s")
    
    @pytest.mark.performance
    @pytest.mark.nonfunctional
    def test_TC021_temps_chargement_dashboard(self, page: Page):
        """
        TC021: Temps de chargement du dashboard
        
        Type: Test de Performance
        Seuil: < 3 secondes
        """
        # Arrange - Se connecter d'abord
        login_page = LoginPage(page)
        login_page.navigate()
        login_page.login_as_valid_user()
        
        # Act - Mesurer le temps de chargement dashboard
        home_page = HomePage(page)
        start_time = time.time()
        home_page.wait_for_load()
        end_time = time.time()
        
        # Calculate
        load_time = end_time - start_time
        
        # Assert
        assert load_time < self.THRESHOLD_PAGE_LOAD, \
            f"Dashboard trop lent: {load_time:.2f}s (seuil: {self.THRESHOLD_PAGE_LOAD}s)"
        
        print(f"\n[PERFORMANCE] Dashboard: {load_time:.2f}s")
    
    @pytest.mark.performance
    @pytest.mark.nonfunctional
    def test_TC022_temps_navigation_pages(self, page: Page):
        """
        TC022: Temps de navigation entre pages
        
        Type: Test de Performance
        Mesure le temps moyen de navigation
        """
        # Arrange
        login_page = LoginPage(page)
        login_page.navigate()
        login_page.login_as_valid_user()
        home_page = HomePage(page)
        home_page.wait_for_load()
        
        navigation_times = []
        pages_to_test = ["training", "student", "dashboard"]
        
        # Act - Naviguer vers plusieurs pages
        for page_path in pages_to_test:
            start_time = time.time()
            page.goto(f"{BasePage.BASE_URL}/{page_path}")
            page.wait_for_load_state("networkidle")
            end_time = time.time()
            navigation_times.append(end_time - start_time)
        
        # Calculate
        avg_time = sum(navigation_times) / len(navigation_times)
        
        # Assert
        assert avg_time < self.THRESHOLD_PAGE_LOAD, \
            f"Navigation moyenne trop lente: {avg_time:.2f}s"
        
        print(f"\n[PERFORMANCE] Navigation moyenne: {avg_time:.2f}s")


class TestCompatibiliteNavigateur:
    """
    Tests de compatibilité navigateur.
    
    Vérifie que l'application fonctionne correctement
    sur différents navigateurs.
    """
    
    @pytest.mark.compatibility
    @pytest.mark.nonfunctional
    def test_TC023_compatibilite_chromium(self, browser: Browser):
        """
        TC023: Compatibilité avec Chromium/Chrome
        
        Type: Test de Compatibilité
        Navigateur: Chromium
        """
        # Ce test s'exécute par défaut avec Chromium
        page = browser.new_page()
        login_page = LoginPage(page)
        
        # Act
        login_page.navigate()
        
        # Assert
        login_page.expect_on_login_page()
        
        print(f"\n[COMPATIBILITÉ] Chromium: OK")
        page.close()
    
    @pytest.mark.compatibility
    @pytest.mark.nonfunctional
    @pytest.mark.skip(reason="Nécessite configuration Firefox")
    def test_TC024_compatibilite_firefox(self, page: Page):
        """
        TC024: Compatibilité avec Firefox
        
        Type: Test de Compatibilité
        Navigateur: Firefox
        
        Note: Exécuter avec --browser firefox
        """
        login_page = LoginPage(page)
        login_page.navigate()
        login_page.expect_on_login_page()
        print(f"\n[COMPATIBILITÉ] Firefox: OK")
    
    @pytest.mark.compatibility
    @pytest.mark.nonfunctional
    @pytest.mark.skip(reason="Nécessite configuration WebKit")
    def test_TC025_compatibilite_webkit(self, page: Page):
        """
        TC025: Compatibilité avec WebKit/Safari
        
        Type: Test de Compatibilité
        Navigateur: WebKit
        
        Note: Exécuter avec --browser webkit
        """
        login_page = LoginPage(page)
        login_page.navigate()
        login_page.expect_on_login_page()
        print(f"\n[COMPATIBILITÉ] WebKit: OK")


class TestErgonomie:
    """
    Tests d'ergonomie et accessibilité basiques.
    """
    
    @pytest.mark.ergonomie
    @pytest.mark.nonfunctional
    def test_TC026_responsive_design_mobile(self, page: Page):
        """
        TC026: Test responsive design (Mobile)
        
        Type: Test d'Ergonomie
        Résolution: 375x667 (iPhone)
        """
        # Arrange - Simuler un écran mobile
        page.set_viewport_size({"width": 375, "height": 667})
        
        login_page = LoginPage(page)
        login_page.navigate()
        
        # Assert - Les éléments doivent être visibles
        login_page.expect_on_login_page()
        
        print(f"\n[ERGONOMIE] Mobile (375x667): OK")
    
    @pytest.mark.ergonomie
    @pytest.mark.nonfunctional
    def test_TC027_responsive_design_tablet(self, page: Page):
        """
        TC027: Test responsive design (Tablette)
        
        Type: Test d'Ergonomie
        Résolution: 768x1024 (iPad)
        """
        # Arrange - Simuler un écran tablette
        page.set_viewport_size({"width": 768, "height": 1024})
        
        login_page = LoginPage(page)
        login_page.navigate()
        
        # Assert
        login_page.expect_on_login_page()
        
        print(f"\n[ERGONOMIE] Tablette (768x1024): OK")
    
    @pytest.mark.ergonomie
    @pytest.mark.nonfunctional
    def test_TC028_responsive_design_desktop(self, page: Page):
        """
        TC028: Test responsive design (Desktop)
        
        Type: Test d'Ergonomie
        Résolution: 1920x1080
        """
        # Arrange - Simuler un écran desktop
        page.set_viewport_size({"width": 1920, "height": 1080})
        
        login_page = LoginPage(page)
        login_page.navigate()
        
        # Assert
        login_page.expect_on_login_page()
        
        print(f"\n[ERGONOMIE] Desktop (1920x1080): OK")
