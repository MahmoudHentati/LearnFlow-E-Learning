"""
Home Page - Page Object Model
LearnFlow E-Training Platform

Page Object pour la page d'accueil/dashboard.
Encapsule les interactions post-connexion.

Généré avec l'assistance de Claude (Anthropic)
"""

from playwright.sync_api import Page, expect
from .base_page import BasePage


class HomePage(BasePage):
    """Page Object pour la page d'accueil/dashboard LearnFlow"""
    
    # Sélecteurs des éléments de la page
    SELECTORS = {
        "welcome_message": ".welcome-message, h1, .user-greeting",
        "logout_button": ".logout-btn, #logout, a[href*='logout']",
        "navigation_menu": "nav, .navbar, .sidebar",
        "trainings_link": "a[href*='training'], .trainings-menu",
        "students_link": "a[href*='student'], .students-menu",
        "profile_link": "a[href*='profile'], .profile-menu",
        "user_name": ".user-name, .username, .user-display-name"
    }
    
    def __init__(self, page: Page):
        """Initialise la page d'accueil."""
        super().__init__(page)
        self.url = f"{self.BASE_URL}/dashboard"
    
    def navigate(self) -> None:
        """Navigue vers le dashboard."""
        self.page.goto(self.url)
        self.wait_for_load()
    
    def get_welcome_message(self) -> str:
        """
        Récupère le message de bienvenue.
        
        Returns:
            Le texte du message de bienvenue
        """
        return self.get_text(self.SELECTORS["welcome_message"])
    
    def get_user_name(self) -> str:
        """
        Récupère le nom de l'utilisateur connecté.
        
        Returns:
            Le nom affiché de l'utilisateur
        """
        return self.get_text(self.SELECTORS["user_name"])
    
    def click_logout(self) -> None:
        """Clique sur le bouton de déconnexion."""
        self.click_element(self.SELECTORS["logout_button"])
    
    def click_trainings(self) -> None:
        """Navigue vers la liste des formations."""
        self.click_element(self.SELECTORS["trainings_link"])
    
    def click_students(self) -> None:
        """Navigue vers la liste des étudiants."""
        self.click_element(self.SELECTORS["students_link"])
    
    def click_profile(self) -> None:
        """Navigue vers le profil utilisateur."""
        self.click_element(self.SELECTORS["profile_link"])
    
    def is_navigation_visible(self) -> bool:
        """
        Vérifie si la navigation est visible.
        
        Returns:
            True si la navigation est visible
        """
        return self.is_visible(self.SELECTORS["navigation_menu"])
    
    def expect_dashboard_loaded(self) -> None:
        """Vérifie que le dashboard est chargé."""
        self.wait_for_element(self.SELECTORS["navigation_menu"])
        expect(self.page.locator(self.SELECTORS["navigation_menu"])).to_be_visible()
    
    def expect_user_logged_in(self) -> None:
        """Vérifie qu'un utilisateur est connecté."""
        expect(self.page.locator(self.SELECTORS["logout_button"])).to_be_visible()
    
    def logout_and_verify(self) -> None:
        """Effectue une déconnexion et vérifie le retour à la page login."""
        self.click_logout()
        self.page.wait_for_url("**/login**", timeout=10000)
