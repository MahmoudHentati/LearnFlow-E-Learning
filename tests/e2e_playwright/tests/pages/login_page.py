"""
Login Page - Page Object Model
LearnFlow E-Training Platform

Page Object pour la page de connexion.
Encapsule toutes les interactions avec la page login.

Généré avec l'assistance de Claude (Anthropic)
"""

from playwright.sync_api import Page, expect
from .base_page import BasePage


class LoginPage(BasePage):
    """Page Object pour la page de connexion LearnFlow"""
    
    # Sélecteurs des éléments de la page
    # À ajuster selon votre implémentation Blazor
    SELECTORS = {
        "email_input": "input[type='email'], input[name='email'], #email",
        "password_input": "input[type='password'], input[name='password'], #password",
        "login_button": "button[type='submit'], .login-btn, #loginBtn",
        "error_message": ".error-message, .alert-danger, .validation-message",
        "register_link": "a[href*='register'], .register-link",
        "forgot_password_link": "a[href*='forgot'], .forgot-password"
    }
    
    def __init__(self, page: Page):
        """Initialise la page de connexion."""
        super().__init__(page)
        self.url = f"{self.BASE_URL}/login"
    
    def navigate(self) -> None:
        """Navigue vers la page de connexion."""
        self.page.goto(self.url)
        self.wait_for_load()
    
    def enter_email(self, email: str) -> None:
        """
        Saisit l'adresse email.
        
        Args:
            email: Adresse email à saisir
        """
        self.fill_input(self.SELECTORS["email_input"], email)
    
    def enter_password(self, password: str) -> None:
        """
        Saisit le mot de passe.
        
        Args:
            password: Mot de passe à saisir
        """
        self.fill_input(self.SELECTORS["password_input"], password)
    
    def click_login(self) -> None:
        """Clique sur le bouton de connexion."""
        self.click_element(self.SELECTORS["login_button"])
    
    def login(self, email: str, password: str) -> None:
        """
        Effectue une connexion complète.
        
        Args:
            email: Adresse email
            password: Mot de passe
        """
        self.enter_email(email)
        self.enter_password(password)
        self.click_login()
    
    def login_as_valid_user(self) -> None:
        """Connexion avec des identifiants valides (test data)."""
        self.login("admin@learnflow.com", "Admin@123")
    
    def login_as_student(self) -> None:
        """Connexion en tant qu'étudiant."""
        self.login("student@learnflow.com", "Student@123")
    
    def login_as_trainer(self) -> None:
        """Connexion en tant que formateur."""
        self.login("trainer@learnflow.com", "Trainer@123")
    
    def get_error_message(self) -> str:
        """
        Récupère le message d'erreur affiché.
        
        Returns:
            Le texte du message d'erreur
        """
        if self.is_visible(self.SELECTORS["error_message"]):
            return self.get_text(self.SELECTORS["error_message"])
        return ""
    
    def is_error_displayed(self) -> bool:
        """
        Vérifie si un message d'erreur est affiché.
        
        Returns:
            True si une erreur est affichée
        """
        return self.is_visible(self.SELECTORS["error_message"])
    
    def click_register_link(self) -> None:
        """Clique sur le lien d'inscription."""
        self.click_element(self.SELECTORS["register_link"])
    
    def click_forgot_password(self) -> None:
        """Clique sur le lien mot de passe oublié."""
        self.click_element(self.SELECTORS["forgot_password_link"])
    
    def expect_login_success(self) -> None:
        """Vérifie que la connexion a réussi (redirection)."""
        self.page.wait_for_url("**/dashboard**", timeout=10000)
    
    def expect_login_failure(self) -> None:
        """Vérifie que la connexion a échoué."""
        expect(self.page.locator(self.SELECTORS["error_message"])).to_be_visible()
    
    def expect_on_login_page(self) -> None:
        """Vérifie qu'on est sur la page de connexion."""
        expect(self.page.locator(self.SELECTORS["email_input"])).to_be_visible()
        expect(self.page.locator(self.SELECTORS["password_input"])).to_be_visible()
