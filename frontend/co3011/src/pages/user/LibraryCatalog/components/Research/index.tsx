import { CustomSlider } from "../../../../../components/slider";
import Widget from "../../../../../components/widget/widget";
import viteLogo from "/vite.svg";

const mockData = [
  {
    title: "The Mathematical Theory of Communication",
    image: viteLogo,
  },
  {
    title: "Computing Machinery and Intelligence",
    image: viteLogo,
  },
  {
    title: "Ideas That Created the Future: Classic Papers of Computer Science",
    image: viteLogo,
  },
  {
    title: "Selected Papers on Computer Science",
    image: viteLogo,
  },
  {
    title: "The Art of Computer Programming, Volume 1",
    image: viteLogo,
  },
];

const responsive = {
  superLargeDesktop: {
    // the naming can be any, depends on you.
    breakpoint: { max: 4000, min: 3000 },
    items: 5,
  },
  desktop: {
    breakpoint: { max: 3000, min: 1024 },
    items: 3,
  },
  tablet: {
    breakpoint: { max: 1024, min: 464 },
    items: 2,
  },
  mobile: {
    breakpoint: { max: 464, min: 0 },
    items: 1,
  },
};

export function Research() {
  return (
    <>
      <h3
        style={{
          fontWeight: 600,
          textTransform: "capitalize",
          marginBottom: 10,
        }}
      >
        research papers & journals
      </h3>

      <CustomSlider
        carouselProps={{
          responsive: responsive,
          children: mockData.map((item) => (
            <Widget marginLeft={30}>
              <p>{item.title}</p>
              <img src={item.image} className="logo" alt="Vite logo" />
            </Widget>
          )),
          infinite: true,
        }}
        style={{
          paddingBlock: 10,
        }}
      />
    </>
  );
}
