from selenium import webdriver
from selenium.webdriver.common.by import By
from time import sleep


def test_login_negative():
    # LOGIN WITH CORRECT USERNAME AND PASSWORD
    login_nav = driver.find_element(By.XPATH, "/html/body/div[1]/div/nav/div/div[2]/a[1]")
    login_nav.click()

    driver.find_element(By.XPATH, "/html/body/div/div/div/form/input").send_keys("user")
    driver.find_element(By.XPATH, "/html/body/div/div/div/form/div/input").send_keys("user1")

    sleep(10)

    login_button = driver.find_element(By.XPATH, "/html/body/div/div/div/form/button")
    login_button.click()
    driver.implicitly_wait(10)
    sleep(10)


def test_login_positive():
    # LOGIN WITH CORRECT USERNAME AND PASSWORD
    login_nav = driver.find_element(By.XPATH, "/html/body/div[1]/div/nav/div/div[2]/a[1]")
    login_nav.click()

    driver.find_element(By.XPATH, "/html/body/div/div/div/form/input").send_keys("user1")
    driver.find_element(By.XPATH, "/html/body/div/div/div/form/div/input").send_keys("user1")

    sleep(10)

    login_button = driver.find_element(By.XPATH, "/html/body/div/div/div/form/button")
    login_button.click()
    driver.implicitly_wait(10)


if __name__ == "__main__":
    # set up
    url = 'http://localhost:8080'
    driver = webdriver.Chrome()
    driver.get(url)
    driver.maximize_window()

    sleep(10)
    test_login_negative()
    test_login_positive()

    driver.close()
