from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions as EC
from time import sleep

from selenium.webdriver.support.wait import WebDriverWait


def scroll_down_page(speed):
    current_scroll_position, new_height = 0, 1
    while current_scroll_position <= new_height and current_scroll_position < 2000:
        current_scroll_position += speed
        driver.execute_script("window.scrollTo(0, {});".format(current_scroll_position))
        new_height = driver.execute_script("return document.body.scrollHeight")


def login():
    # LOGIN WITH CORRECT USERNAME AND PASSWORD
    login_nav = driver.find_element(By.XPATH, "/html/body/div[1]/div/nav/div/div[2]/a[1]")
    login_nav.click()

    driver.find_element(By.XPATH, "/html/body/div/div/div/form/input").send_keys("user1")
    driver.find_element(By.XPATH, "/html/body/div/div/div/form/div/input").send_keys("user1")

    sleep(10)

    login_button = driver.find_element(By.XPATH, "/html/body/div/div/div/form/button")
    login_button.click()
    driver.implicitly_wait(5)
    sleep(10)


def search_offers():
    adults_number = 2

    # BUY FIRST OFFER
    adults_input = driver.find_element(By.XPATH, "/html/body/div[1]/div/div[2]/form/div[4]/input")
    adults_input.send_keys(Keys.CONTROL + "a")
    adults_input.send_keys(Keys.DELETE)
    adults_input.send_keys(adults_number)
    search_button = driver.find_element(By.XPATH, "/html/body/div[1]/div/div[2]/form/input")
    sleep(10)
    search_button.click()
    sleep(10)
    driver.implicitly_wait(5)
    check_offer_button = driver.find_element(By.XPATH, "/html/body/div[1]/div/div/ul/li[1]/button")
    check_offer_button.click()

    scroll_down_page(100)
    sleep(10)


def test_make_reservation_positive():
    login()
    main_page = driver.find_element(By.XPATH, "/html/body/div[1]/div/nav/div/div[1]/a[1]")
    main_page.click()
    search_offers()
    sale = driver.find_element(By.XPATH, "/html/body/div/div/div/div[1]/form/input[1]")
    sale.send_keys(Keys.DELETE)
    sale.send_keys("PROMOCJA")
    sleep(10)
    wait = WebDriverWait(driver, 100)
    wait.until(EC.element_to_be_clickable((By.XPATH, "/html/body/div/div/div/div[1]/form/input[2]")))
    reserve_button = driver.find_element(By.XPATH, "/html/body/div/div/div/div[1]/form/input[2]")
    reserve_button.click()

    sleep(10)
    wait.until(EC.element_to_be_clickable((By.XPATH, "/html/body/div/div/div/div/form/input")))
    card_input = driver.find_element(By.XPATH, "/html/body/div/div/div/div/form/input")
    card_input.clear()
    card_input.send_keys("1111111111111111")
    sleep(61)
    card_input.submit()
    sleep(10)


def test_show_user_trips():
    user_trips = driver.find_element(By.XPATH, "/html/body/div[1]/div/nav/div/div[1]/a[3]")
    user_trips.click()
    sleep(10)


if __name__ == "__main__":
    # set up
    url = 'http://localhost:8080'
    driver = webdriver.Chrome()
    driver.get(url)
    driver.maximize_window()
    sleep(10)
    # execute user stories
    test_make_reservation_positive()
    test_show_user_trips()

    driver.close()
