const isMauiApp = (): boolean => {
  return new URLSearchParams(window.location.search).get('platform') === 'maui'
}

export const openNativeAboutPage = (): void => {
  if (!isMauiApp()) {
    console.log('Native navigation is only available inside the MAUI application.')
    return
  }

  window.location.href = 'maui://about'
}
